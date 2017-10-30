# The fantastic four coding patterns of Continuous Delivery

In this repo you find an example in C# of the coding patterns derscribed in the InfoQ article [Continuous Delivery Coding Patterns: Latent-to-Live Code & Forward Compatible Interim Versions](https://www.infoq.com/articles/continuous-delivery-coding-patterns).

Below you find a description of the patterns applied in sequence.
Files for each version are organised in a different folders, to make it easier to download and inspect each version off-line.

## Version 1-Initial-code

This is the initial version that simulates tyre degradation using only a linear degradation per lap and the ideal lap time.

- **Backward compativility breaking changes: none.**

- **Rollback version: v0.**
This is the previous official and well functioning version that can be used to replace this version in case a show-stopper bug shows-up.

- **Supported Db version: v10.**
The file that stores ideal laptime and the degradation per lap.

## Version 2a-Latent-code

This version is the first step in extending the tyre degradation simulation taking into account tyre's temperature.
It consists in adding side-by-side the new simulation paramiter in TyreDegradationParameters, and the new simulation in TyreDegradationSimulator, both as latent code.

- **Backward compativility breaking changes: none.**

- **Rollback version: v1.**

- **Supported Db version: v10.**

## Version 2b-Latent-to-live-code

In TyreDegradationSimulator, latent code to live, call the overload

## Version 2c-Latent-to-live-code+feature-toggle

In Program, read 
- Degradation per operating temperature delta and
- Simulation operating temperature delta

## Version 3a-Forward-compatible-interim-version

In storage, delivered with db v10
with auto fallback feature toggle

## Version 3b-Forward-compatible-interim-version

Same as before, delivered with db v11

## Version 4-Final-code

Notes (aim, differences from the previous version, etc).

