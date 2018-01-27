# The fantastic four coding patterns of Continuous Delivery

In this repo you find an example in C# of the coding patterns described in the InfoQ article [Continuous Delivery Coding Patterns: Latent-to-Live Code & Forward Compatible Interim Versions](https://www.infoq.com/articles/continuous-delivery-coding-patterns).

Below you find a description of the patterns applied in sequence.
Source code for each version are organised in different folders, to make it easier to download and inspect each version off-line.

## Version 1-Initial-code

This is the initial version that simulates tyre degradation using only a linear degradation per lap and the ideal lap time.

- **Backward compatibility breaking changes: none.**

- **Rollback version: v0.**
Version 0 is the previous official and well functioning version that can be used to replace this version in case a show-stopper bug shows-up.

- **Supported Storage version: v10.**
The current v10 file format version for the file that stores ideal lap time and the degradation per lap.

## Version 2a-Latent-code

This version is the first step in extending the tyre degradation simulation taking into account tyre's temperature.
It consists in adding side-by-side the new simulation parameter in TyreDegradationParameters, and the new simulation in TyreDegradationSimulator, both as latent code.

The new code added can be automatically tested, while keeping the system working as before and in a releasable state at any time.

- **Backward compatibility breaking changes: none.**

- **Rollback version: v1.**

- **Supported Storage version: v10.**

## Version 2b-Latent-to-live-code

The side-by-side code in TyreDegradationParameters and TyreDegradationSimulator previously added and tested, is now partially used in production replacing the original simulation code: it is executed using temperature parameters set to zero so that temperature has no effect on the tyre degradation simulation.

In this way the new code can be verified making sure that it does not introduce any bug to the existing simulation model running in production.

- **Backward compatibility breaking changes: none.**

- **Rollback version: v2a.**

- **Supported Storage version: v10.**


## Version 2c-Latent-to-live-code + toggle

This version includes additional code in Program to inquire also for the new simulation parameters of the temperature.
It also includes additional code in Storage to persist and retrieve the temperature parameter (leading to the v11 of the storage).

Because the storage version v11 break backwards compatibility, this would make it "impossible" to rollback to the previous version in case of a showstopper bug, leaving us without a viable remediation plan. For that reason, a feature toggle is introduced here to switch the new feature that uses v11 on and off, making it possible to:
- test the new version working on storage v11 in the dev/test environment 
- continue testing the latent-to-live code and the last latent changes in the prod environment, 
- release this version with the toggle off to make it work with storage v10.

This version with the feature toggle in this way becames a step-stone for creating a forward compatible version (next version 3a) to manage the breaking changes on the storage. 

- **Backward compatibility breaking changes: none (yes with toggle on).**

- **Rollback version: v2b (none with toggle on).**

- **Supported Storage version: v10 (v11 with toggle on).**

Note that the changes from v10 to v11 here are used to simulate a change that breaks backwards compatibility in a way that makes it impractical to rollback to the previous version as part of an automated remediation plan.

This Storage is used here to represent a system component that once updated from version v10 to a backward incompatible version v11, in the event of a showstopper bug it cannot be rolled back at all or quickly enough to avoid a disruption for the users.
It could be for example:
- a relational database with a large amount of data, updated to v11 by a DML command that cannot be easily undone; or   
- a service in the cloud or an AWS Lambda function with an API that changes between v10 and v11, that cannot stay live with both versions at the same time, and with a large number of different client apps that cannot be easily rolled back to v10 all together.

Can you think of backward incompatible changes in your codebase that are hard to rollback?

## Version 3a-Forward-compatible-interim-version

In this version, Storage can detect the current version in use, and consequently adapt its behaviour to both versions v10 and v11.
Based on the current version detected by Storage, Program configures itself accordingly exposing the new feature when v11 is in user or hiding it otherwise. Feature toggles are not required anymore.

This version is initially released in production and used with v10, until it can be confirmed to be a reliable version to be used as last official version for the rollback, before moving to version v11.

- **Backward compatibility breaking changes: none.**

- **Rollback version: v2c.**

- **Supported Storage version: v10 and v11, used with v10.**

## Version 3b-Forward-compatible-interim-version

Exactly the same version as 3a, just released with Storage v11.

- **Backward compatibility breaking changes: none up to 3a, the Storage for versions prior 3a.**

- **Rollback version: v3a.**

- **Supported Storage version: v10 and v11, used with v11.**

## Version 4-Final-code

The extension of the tyre simulation is now completed with a model that takes into account the operating temperature of the tyre. Even if this change required a breaking change to the storage, the patterns employed enabled us to implement the change gradually and rollback to the previous version at any time, safely and quickly.

The code needed for the forward-compatibility has been removed from everywhere including the tests.
This version now supports only storage version v11. Still, in case of showstopper bugs, it is possible to rollback to the previous version.


- **Backward compatibility breaking changes: none.**

- **Rollback version: v3b.**

- **Supported Storage version: v11.**







