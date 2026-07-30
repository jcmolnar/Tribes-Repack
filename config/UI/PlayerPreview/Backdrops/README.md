# Preview backdrop images

Drop `.png`, `.gif` (animated is fine) or `.bmp` files here and reference them from
`..\backdrops.json`.

★This folder is the ONLY place the client will read a backdrop image from.★ An `asset` path that
resolves anywhere else is refused at load and the entry is dropped with a reason on the console --
a manifest travels with mods, and one that could name any path would be a file-read primitive
with a menu in front of it.

See `..\BACKDROP_AUTHORING.md`.
