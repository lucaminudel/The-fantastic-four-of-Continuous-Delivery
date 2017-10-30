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

The new code added can be automatically tested, while keeping the system working as before and in a releasable state at any time.

- **Backward compativility breaking changes: none.**

- **Rollback version: v1.**

- **Supported Db version: v10.**

## Version 2b-Latent-to-live-code

Side-by-side code in TyreDegradationParameters and TyreDegradationSimulator previously added and tested, is now partially used in production replacing the original simulation code: it is executed using temperature parameters set to zero so that temperature has no effect on the tyre degradation.

In this way the new code can be verified that it does not introduce any bug to the existing simulation model.

- **Backward compativility breaking changes: none.**

- **Rollback version: v2a.**

- **Supported Db version: v10.**


## Version 2c-Latent-to-live-code + toggle

Code is added to Program, in order to inquire also for the simulation parameters of the temperature.
Code is also added to Storage to persist and retrieve the temperature parameters (introducing the v11 of the db).

At this stage this new code is supposed to be used only in development and test mode and **not** in production.
For this reason a feature toggle is added, and it must be switched off in the release.

- **Backward compativility breaking changes: none.**

- **Rollback version: v2b.**

- **Supported Db version: v10.**


## Version 3a-Forward-compatible-interim-version

In storage, delivered with db v10
with auto fallback feature toggle

## Version 3b-Forward-compatible-interim-version

Same as before, delivered with db v11

## Version 4-Final-code

Notes (aim, differences from the previous version, etc).

