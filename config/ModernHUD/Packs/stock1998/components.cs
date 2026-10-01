// Shared stock-style ScriptGL panels, available to every pack's provider picker.
// These are full renderers. Unlike the legacy chat/radar decorations, selecting
// one yields the shared base panel so it is drawn exactly once.

function stock1998::healthenergy(%screen)
{
   ModernHUDStock::drawHealth(%screen);
   ModernHUDStock::drawEnergy(%screen);
}

function stock1998::weapon(%screen)
{
   ModernHUDStock::drawWeapon(%screen);
}

function stock1998::clock(%screen)
{
   ModernHUDStock::drawClock(%screen);
}

function stock1998::chat(%screen)
{
   ModernHUDStock::drawChat(%screen);
}

function stock1998::minimap(%screen)
{
   ModernHUDStock::drawMinimap(%screen);
}

ModernHUD::component("stock1998", "healthenergy", "healthenergy", "stock1998::healthenergy");
ModernHUD::component("stock1998", "weapon", "weapon", "stock1998::weapon");
ModernHUD::component("stock1998", "clock", "clock", "stock1998::clock");
ModernHUD::component("stock1998", "chat", "chat", "stock1998::chat");
ModernHUD::component("stock1998", "minimap", "minimap", "stock1998::minimap");
