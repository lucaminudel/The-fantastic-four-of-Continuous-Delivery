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

This version includes additional code in Program to inquire also for the simulation parameters of the temperature.
It also includes additional code i Storage to persist and retrieve the temperature parameters (introducing the v11 of the db).

Because the db version v11 break beckward compatibility, this would make it "impossible" to rollback to the previous version in case of a showstopper bug, leaving us without a viable remediation plan. For that reason, a feature toggle is introduced here to switch the new feature on and off, making it possible to:
- test the new version working on db v11 in the dev/test environment 
- continue testing the latent-to-live code and the last latent changes in the prod environment, 
- release this verion with the toggle off to keep it working with db v10.

This version is a step-stone for creating a forward compatible version to manage the breaking changes on the db. 

- **Backward compativility breaking changes: none (yes with toggle on).**

- **Rollback version: v2b (none with toggle on).**

- **Supported Db version: v10 (v11 with toggle on).**

Note that the changes from file format v10 to v11 here are used to simulate a change that breaks backwards compatibility in a way that makes it impractical to rollback to the previous version as part of an automated remediation plan.
An example of a real case is large SQL db storage that makes it extremely slow (because of the quantity of data), or difficult (because of the number of different clients affected) or impossible (because of irreversible schema changes) to rollback to the previous version of the db schema.
Can you think of similar changes that you faced in the past or that could affect your system in the future?

## Version 3a-Forward-compatible-interim-version

In storage, delivered with db v10
with auto fallback feature toggle

- **Backward compativility breaking changes: none.**

- **Rollback version: v2c.**

- **Supported Db version: v10, v11.**

## Version 3b-Forward-compatible-interim-version

Same as before, delivered with db v11

## Version 4-Final-code

Notes (aim, differences from the previous version, etc).

