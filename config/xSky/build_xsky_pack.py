#!/usr/bin/env python3
"""Build an xSky sky FOLDER from an equirectangular panorama.

Player workflow (see README.txt):
    1. Make a folder here named after your sky (letters/digits/underscore only,
       no dots):            config\\xSky\\MySky\\
    2. Drop a panorama in:  config\\xSky\\MySky\\MySky.png   (2:1 equirect)
    3. Run:                 python build_xsky_pack.py MySky
       or just              python build_xsky_pack.py        (builds every
                            folder that has a panorama but no built sky yet)
    4. Restart the game (or run xSky::rescan(); in the console) and pick it in
       Options > Graphics > Sky.

Output goes INTO the sky's folder: six cube faces, <Name>_sky.dml,
<Name>_sky.png (menu preview) and <Name>_sky.cs (rotation/speed/haze
defaults) -- the game loads the folder directly, no zip needed.
"""

from __future__ import annotations

import argparse
import io
import math
import re
import sys
from pathlib import Path

import numpy as np
from PIL import Image

FACE_NAMES = ("left", "front", "right", "back", "top", "bottom")
TEMPLATE_NAME = "Cloudscape"          # its _sky.dml is the binary template
HERE = Path(__file__).resolve().parent


def make_wrap_seamless(image: Image.Image) -> Image.Image:
    # Keep the source resolution (HD panoramas stay HD); only enforce 2:1.
    image = image.convert("RGB")
    width = max(2048, image.width)
    height = width // 2
    if (image.width, image.height) != (width, height):
        image = image.resize((width, height), Image.Resampling.LANCZOS)
    pixels = np.asarray(image, dtype=np.float32).copy()
    blend = max(32, width // 24)

    for offset in range(blend):
        weight = 0.5 * (1.0 + math.cos(math.pi * offset / (blend - 1)))
        left = pixels[:, offset, :].copy()
        right = pixels[:, width - 1 - offset, :].copy()
        average = (left + right) * 0.5
        pixels[:, offset, :] = left * (1.0 - weight) + average * weight
        pixels[:, width - 1 - offset, :] = right * (1.0 - weight) + average * weight

    return Image.fromarray(np.clip(pixels, 0, 255).astype(np.uint8), "RGB")


def sample_equirect(panorama: np.ndarray, x: np.ndarray, y: np.ndarray, z: np.ndarray) -> np.ndarray:
    height, width, _ = panorama.shape
    length = np.sqrt(x * x + y * y + z * z)
    longitude = np.arctan2(x, y)
    latitude = np.arcsin(np.clip(z / length, -1.0, 1.0))

    source_x = np.mod((0.5 + longitude / (2.0 * math.pi)) * width, width)
    source_y = np.clip((0.5 - latitude / math.pi) * (height - 1), 0, height - 1)

    x0 = np.floor(source_x).astype(np.int32)
    y0 = np.floor(source_y).astype(np.int32)
    x1 = (x0 + 1) % width
    y1 = np.minimum(y0 + 1, height - 1)
    tx = (source_x - x0)[..., None]
    ty = (source_y - y0)[..., None]

    top = panorama[y0, x0] * (1.0 - tx) + panorama[y0, x1] * tx
    bottom = panorama[y1, x0] * (1.0 - tx) + panorama[y1, x1] * tx
    return np.clip(top * (1.0 - ty) + bottom * ty, 0, 255).astype(np.uint8)


def cube_faces(panorama: Image.Image, size: int = 1024) -> dict:
    source = np.asarray(panorama, dtype=np.float32)
    axis = np.linspace(-1.0, 1.0, size, dtype=np.float32)
    a, b = np.meshgrid(axis, axis)
    one = np.ones_like(a)

    # Matches rt.cpp s_skyQuad and DML order exactly. b=-1 is the first/top bitmap row.
    directions = {
        "left": (-one, a, -b),
        "front": (a, one, -b),
        "right": (one, -a, -b),
        "back": (-a, -one, -b),
        "top": (a, b, one),
        "bottom": (a, -b, -one),
    }
    return {
        name: Image.fromarray(sample_equirect(source, *directions[name]), "RGB")
        for name in FACE_NAMES
    }


def build_dml(template: bytes, pack_name: str) -> bytes:
    if not re.fullmatch(r"[A-Za-z0-9_]+", pack_name):
        raise ValueError("Sky name must contain only letters, digits, and underscores")

    data = bytearray(template)
    for face in FACE_NAMES:
        old = f"{TEMPLATE_NAME}_{face}.bmp".encode("ascii")
        new = f"{pack_name}_{face}.bmp".encode("ascii")
        if len(new) >= 32:
            raise ValueError(f"DML filename is too long: {new.decode()}")
        start = data.index(old)
        data[start : start + 32] = new + b"\0" * (32 - len(new))
    return bytes(data)


def horizon_haze(panorama: Image.Image) -> tuple:
    pixels = np.asarray(panorama)
    center = pixels.shape[0] // 2
    band = pixels[center - 3 : center + 4].reshape(-1, 3)
    return tuple(int(value) for value in np.median(band, axis=0))


def build_sky(folder: Path) -> None:
    name = folder.name
    source = folder / f"{name}.png"
    if not source.exists():
        for ext in (".jpg", ".jpeg"):
            alt = folder / f"{name}{ext}"
            if alt.exists():
                source = alt
                break
    if not source.exists():
        raise FileNotFoundError(f"{folder}: no panorama named {name}.png / .jpg")

    template_dml = (HERE / TEMPLATE_NAME / f"{TEMPLATE_NAME}_sky.dml").read_bytes()

    with Image.open(source) as src:
        panorama = make_wrap_seamless(src)
    faces = cube_faces(panorama)
    haze = horizon_haze(panorama)

    (folder / f"{name}_sky.dml").write_bytes(build_dml(template_dml, name))
    preview = panorama.resize((275, 140), Image.Resampling.LANCZOS)
    preview.save(folder / f"{name}_sky.png", "PNG", optimize=True)
    for face in FACE_NAMES:
        faces[face].save(folder / f"{name}_{face}.png", "PNG", optimize=True, compress_level=7)
    (folder / f"{name}_sky.cs").write_bytes(
        (
            "$xSky::Settings::Rotation = 0;\r\n"
            "$xSky::Settings::Speed = 0;\r\n"
            f'$xSky::Settings::Haze = "{haze[0]} {haze[1]} {haze[2]}";\r\n'
        ).encode("ascii")
    )
    print(f"built {name}: 6 faces @1024, preview, dml, cs (haze {haze})")


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("names", nargs="*", help="sky folder name(s); default = every unbuilt folder")
    ap.add_argument("--rebuild", action="store_true", help="rebuild even if the sky is already built")
    args = ap.parse_args()

    targets = []
    if args.names:
        targets = [HERE / n for n in args.names]
    else:
        for d in sorted(HERE.iterdir()):
            if not d.is_dir() or d.name == TEMPLATE_NAME:
                continue
            built = (d / f"{d.name}_sky.dml").exists()
            if built and not args.rebuild:
                continue
            targets.append(d)

    if not targets:
        print("nothing to build (every sky folder already has a built _sky.dml)")
        return 0

    failed = 0
    for folder in targets:
        try:
            build_sky(folder)
        except Exception as exc:  # keep going; report at the end
            print(f"FAILED {folder.name}: {exc}", file=sys.stderr)
            failed += 1
    return 1 if failed else 0


if __name__ == "__main__":
    raise SystemExit(main())
