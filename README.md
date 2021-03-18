# Jak Audio Tool

A tool that intends to handle the VAG directory of Jak and Daxter games.

I am still working on the basics, so contributions from strangers wouldn't make much sense, at least for now. Eventually it will be good enough for others to contribute.

## Details

Supports Jak TPL and Jak II VAGDIR.AYB files for now. Jak 3/X support is limited (it can get some data, but not everything).

Has a working command line application, a GUI application is being worked on. They both use the same library so function-wise they will be the same, but GUI application will be more user-friendly.

## What can it do?
- Print VAGDIR data to console.
- Print VAGDIR data to text file.
- Build VAGDIR based on a text file.

## Todo:
- Reverse the VAG name packing algorithm used in Jak 3 (hopefully Jak X uses the same algorithm).
- Add ability to split and (re)build VAGWAD files.

## Future plans:
- Make our own Sony ADPCM converter (to wav and from wav), or add an already existing one into this project.
- Support SBK files. 
