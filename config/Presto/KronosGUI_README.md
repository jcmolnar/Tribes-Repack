# Kronos modern GUI — scaling & movable-panel design notes

Lives alongside the scripts it documents (`config/Presto/`). Files:

- **KronosMenu.cs** — TAB menu (Options + Players + character-info), the UI-scale
  slider, and the shared **scale + drag framework** used by all the Kronos GUI.
- **KronosShop.cs** — shop / inventory / BANK screen (reuses the menu's scale + positions).
  Long lists SCROLL via a right-side scrollbar per pane (drag ids `sbinv` / `sbst`,
  high-transparency track+thumb) — no more pages. Max `$KS::MaxRows` rows visible;
  `$KS::scroll[inv|st]` is the offset. Modes (`$KS::open`): `inv` (I-key, one pane),
  `shop` (merchant, Buy/Sell/Use/Drop), `bank` (left = your ItemData equipment, right =
  bank storage; **Deposit/Withdraw** items + **Dep/W-D All $** coins). Bumping a banker
  opens `bank` for HUD clients (server `KronosBank_*` in KronosHUD_Server.cs, mirroring the
  economy.cs BankStorage logic; vanilla keeps the stock banker menu). Belt/backpack bank
  storage + partial coin amounts are a follow-up.
- **KronosNPC.cs** — window over the EXISTING NPC dialogue (does NOT invent dialogue).
  The bot's real spoken lines arrive via `KNPCLine`; the clickable options arrive
  pre-parsed via `KNPCOpts` (server-side, see below) — the client just renders them and
  on click sends `#say keyword`. A line with options replaces the buttons; a flavor line
  leaves them, so a stray click never strands you. Clicking a button sends
  `remoteEval(2048, say, 0, "#say keyword")` — the real comchat dialogue logic (quest
  hand-ins, enemy spawns, teleporters, generic SAY/NSAY) runs unchanged. Draggable by its
  title bar (id `knpcwin`, persisted `npcX/npcY`). `KronosNPC::test()` previews it.
- **KronosNPC_Server.cs** (SERVER, Dev `rpg/scripts/`) — thin bridge only. `KronosNPC_Open`
  (called from `Player::onCollision` for generic non bank/shop/ascension town bots, HUD
  clients only) opens the window + cursor; the client then auto-sends `#say hi` so the
  bot's real greeting fires. **`Ai.cs` `AI::sayLater` is hooked** to mirror every bot line
  to the window (`KNPCLine`) + the parsed options (`KNPCOpts`) while `%client.knpcWinOpen`
  is set. `KronosNPC_ExtractOpts` pulls the player's phrases from each line two ways:
  bracketed `[keyword]` and ALL-CAPS words (`ENTER`/`YES`/`NO`/`BUY`...). Case detection
  MUST use `String::Compare` (case-sensitive): `String::findSubStr` is case-INSENSITIVE
  here and `==` numerically coerces letters (`'E'=='0'` bug) — both corrupt caps parsing.
  No per-bot dialogue here. BRACKETS WIN: if a line has any `[keyword]`, only those are used
  (caps ignored). The comchat botType prompts were edited to bracket their options
  (`[ENTER]`, `[FIGHT]`/`[LEAVE]`, `[YES]`/`[NO]`, `[BUY]`, `[SMITH]`, class & per-house
  lists, ...) for 100% clean capture; any un-bracketed prompt still falls back to caps.
- **KronosHUD.cs** — vhud HUD (HP/MP/XP, Lv/Gold, cast bar, weapon popup, equipped
  weapon bar, target frame). Loads first; defines `kronos::examine_render` and the
  `onPreDraw` override that feeds vhud the real canvas size. Vitals bar text is drawn by
  `kronos::drawBarText` (centered, font auto-shrinks so big RPG numbers stay inside the
  bar); XP text is near-black for contrast on the gold bar; the vitals panel has no
  backdrop (bars provide their own dark track).
- **KronosInput.cs** — reusable ScriptGL **text-input field** (keyboard capture).
  ScriptGL is draw-only, so this rides a small NATIVE-BUILD seam: `glTextInput(1)`
  makes the engine forward each keyboard MAKE to `ScriptGL::onChar`/`onKey` and
  SWALLOW it (so binds don't fire while typing), `glTextInput(0)` stops. Owns the
  ONE focused field (buffer + caret); consumers call `KronosInput::focus(id,
  initial, submitFn, cancelFn, navFn, maxLen)`, draw the focused field with
  `KronosInput::drawText`, and implement submit/cancel/nav. Used by the chat
  composer (and ready for bank-amount / search boxes). **Requires the native
  build** (engine/SimGui/code/scriptGL.cpp + engine/Sim/code/simGame.cpp); on a
  client without the seam `glTextInput` is just an unknown command and typing
  does nothing — the rest of the GUI is unaffected. The engine sends the literal
  CHARACTER to `onChar` (TorqueScript can't convert an ascii code back to a char).
- **scriptgl2.cs** — stock Hudbot vhud framework (NOT a Kronos file; don't edit).

Load order (autoexec.cs): scriptgl2 → KronosHUD → KronosInput → KronosMenu → KronosShop → KronosChat → KronosNPC.
Server (Dev `rpg/scripts/`, Server.cs): KronosHUD_Server, KronosNPC_Server.

## Rendering seam
ScriptGL calls `onPreDraw`/`onPostDraw` with `%dimensions`. That value can be stale
in windowed OpenGL, so we ignore it and use **`Control::getExtent(PlayGui)`** — the
real canvas the GL ortho draws into — via `KronosMenu::screenDim()` (falls back to the
passed dims if the extent isn't sane yet). All layout/drawing is in real screen pixels.

## Scaling model (reference + knob)
Pure-proportional layout makes the GUI a fixed *percent* of the screen (looks identical
at every resolution). Instead, SIZES scale toward a reference height so the GUI takes a
smaller share of the screen at high res; POSITIONS anchor to the real screen.

`KronosMenu::uiScale(sh)` returns factor `k = min(1.0, (UiRefH/sh) * UiScale)`:
- capped at 1.0 → never bigger than the original proportional size (safe in small windows)
- e.g. 2560x1440 @ UiScale 1.0 → k=0.75 (sized for 1080p); 809x597 → k=1.0 (proportional)

computeLayout uses `szW=sw*k, szH=sh*k` for sizes (pad, widths, row/title/line heights,
fonts) and raw sw/sh for anchors.

## Movable panels (drag + persist)
Drag works only while the cursor is up (press **TAB**, or open a shop). Panels stash
their on-screen rects each frame; `onMouseLMB` checks **slider → panel drag → click**.

- **UI-scale slider** (KronosMenu, drag id `uislider`): the track/knob adjusts the scale;
  grabbing the rest of the widget MOVES it (persisted in `sliderX/sliderY`).
- TAB-menu panels (KronosMenu): **menu** + **players** drag by their **title bar**;
  **character-info** box drags anywhere. Positions are screen fractions.
- Shop panes reuse the menu/players positions (drag from either screen moves both).
- vhud HUD panels (KronosHUD): **kh_vitals** (HP/MP/XP), **kh_info** (Lv/Gold),
  **kh_wbar** (equipped weapon) — whole panel is the handle; hit-tested via their vhud
  render rects; on drag we rewrite `$vhud[name,pos]` + bust `$vhud[name,lastdimensions]`.
  Their SIZE stays proportional (old framework) — only position is movable.
- **Chat** (KronosChat.cs): a **custom ScriptGL chat overlay** replaces the stock engine
  chat. Incoming chat is captured via Presto's `eventClientMessage` (from `onClientMessage`)
  and rendered with `glDrawString` at an **adjustable font size + visible line count**,
  word-wrapped, draggable (drag id `kchat`, whole box), and always on-screen. The stock
  `chatDisplayHud` is hidden via `Control::SetVisible` (re-applied on PlayGui-open /
  resolution-change). Why a custom overlay: the stock chat's fonts are fixed bitmap `.pft`
  files with no script-side size control.
  - `KronosChat::disable()` restores the stock chat (and the old stock-chat grip path: a
    "Chat" grip tab that moves `chatDisplayHud` via `Control::setPosition`, persisted in
    `chatX/chatY`, stock pos captured in `chatStockX/Y`). Only active when the overlay is off.
  - **Window size and text size are independent.**
    - **Resize grip** (drag id `kchatsz`, bottom-right corner): drag to set the WINDOW
      width (X) and height (Y). How many lines show = window height / line height.
    - **A- / A+ buttons** (top-right, click): change the TEXT size (`chatFontH`), independent
      of the window. (`KronosChat::setFont/smaller/bigger` still work too.)
    - Body-drag still moves the box (`kchat`).
  - **Scrollbar** (drag id `kchatscr`, right gutter): high-transparency track + thumb, shown
    while the cursor is up. Drag it to scroll back through history (`$KC::scroll` lines from
    the bottom). New messages keep the view anchored while scrolled; scroll resets to newest
    when the cursor hides. Gutter width is always reserved so wrapping doesn't jump.
  - **Composer (chat input)** — a one-line input row drawn just below the chat box
    (shown while the cursor is up, or while actively typing). Type and press **Enter**
    to send, **Esc** to cancel, **Up/Down** to recall sent-message history. An **All/Team**
    chip on the left toggles the channel; the line is sent with
    `remoteEval(2048, Say, team, text)` (senderName left empty — a non-empty one trips
    `remoteSay`'s exploit guard; team 0 = all, 1 = team). Click the row to focus it, or
    bind a key to `KronosChat::beginSay()` for talk-key-style quick chat — the key seam
    is global so it works even with the cursor down (typing suppresses movement/weapon
    binds until Enter/Esc). Built on KronosInput.cs, so it needs the native glTextInput
    seam; without it the row shows but typing does nothing.
  - Caveat: a few engine-LOCAL messages go straight to the C++ chat control and never reach
    `onClientMessage`, so they won't appear in the overlay.

Transient panels (cast bar / weapon popup / target frame) are NOT draggable — they only
show briefly during gameplay when the cursor is down. Would need a "HUD edit mode".

## Persisted prefs (saved in ClientPrefs.cs)
```
$pref::Kronos::UiScale      scale knob (1.0 = sized for UiRefH; lower = smaller)
$pref::Kronos::UiRefH       reference height (default 1080)
$pref::Kronos::menuX/menuY        menu panel (+ shop "your items" pane), fractions
$pref::Kronos::playersX/playersY  players panel (+ shop merchant pane), fractions
$pref::Kronos::infoX/infoY        character-info / examine box; infoX="c" = centered
$pref::Kronos::sliderX/sliderY    UI-scale slider widget pos; sliderX="c" = centered
$pref::Kronos::npcX/npcY          NPC dialogue window pos; npcX="c" = centered
$pref::Kronos::npcW               NPC dialogue window width, fraction of (scaled) screen
$pref::Kronos::vitalsPos          vhud kh_vitals  "x y" percent
$pref::Kronos::infoHudPos         vhud kh_info    "x y" percent
$pref::Kronos::wbarPos            vhud kh_wbar    "x y" percent
$pref::Kronos::chatX/chatY        STOCK chat window pos, fractions ("" = unmoved)
$pref::Kronos::chatStockX/Y       captured stock chat position (for reset)
--- custom chat overlay (KronosChat.cs) ---
$pref::Kronos::chatEnabled        true = overlay (stock hidden); false = stock chat
$pref::Kronos::chatFontH          TEXT size: font height / screen height (default 0.011)
$pref::Kronos::chatW              WINDOW width, fraction of screen (default 0.34)
$pref::Kronos::chatH              WINDOW height, fraction of screen (default 0.16)
$pref::Kronos::chatPosX/chatPosY  overlay position, fractions (default 0.015, 0.58)
$pref::Kronos::chatBg             dim backdrop behind chat text (default true)
```

## Console helpers (KronosMenu.cs)
```
KronosMenu::setScale(0.85)   set + persist the UI scale (the slider does this live)
KronosMenu::resetLayout()    restore all default panel positions (incl. chat); leaves scale
KronosMenu::probe()          (run with TAB open) dumps dims, scale factor k, positions

KronosChat::setFont(0.010)   chat TEXT size (fraction of screen height; lower = smaller)
KronosChat::smaller()/bigger()   nudge chat text size by 0.001
KronosChat::setLines(12)     size the WINDOW height to fit n lines (at current text size)
KronosChat::setWidth(0.30)   chat WINDOW width (fraction of screen)
KronosChat::disable()/enable()   toggle custom overlay vs stock chat
KronosChat::clear()          wipe chat history;  KronosChat::test()  inject sample lines
KronosChat::beginSay()       focus the chat composer (bind to a key for quick chat)
KronosChat::toggleTeam()     switch the composer between All / Team channel
```
Bind quick-chat to a key (native build only — needs the glTextInput seam):
```
bindCommand(keyboard0, make, "u", TO, "KronosChat::beginSay();");
KronosNPC::test()            preview the NPC dialogue window;  KronosNPC::resetPos()
```

## Gotchas
- vhud caches per-panel by `lastdimensions`; to move a vhud panel you MUST also set
  `$vhud[name,lastdimensions]=""` so it recomputes.
- `Control::getExtent`/`getPosition` return `"w h"` / `"x y"`; `Control::setPosition`
  (Hudbot, default HUDs) takes `(name, x, y)`.
- Keep this file and the .cs in sync between `C:\Dynamix\Tribes\config\Presto` and the
  Development repo's `config\Presto`.
