# ORE ID Unity Test Console

## Overview

This project lays the groundwork for implementing blockchain actions within the popular game development platform.  The test console demonstrates ORE ID Custodial API functionality:

* Custodial ORE ID account creation -> Easily provision blockchain accounts which the developer holds the keys to.
* Custodial blockchain account creation -> Add additional blockchain accounts to a custodial user's wallet.
* Custodial transaction signing -> Sign and broadcast transactions for a custodial user.


## Docs

* [ORE ID Unity Integration documentation](https://docs.oreid.io/ore-id/build/integration/unity)
* [ORE ID Custodial API documentation](https://docs.oreid.io/ore-id/build/features/custodial)
* [ORE ID Custodial API Postman documentation](https://documenter.getpostman.com/view/7805568/SWE55yRe#eff54202-abd1-4f0c-93d1-237eb660fe27)


## Prerequisites

1. Register an ORE ID dApp using the [developer dashboard](https://oreid.io/developer/new-app).
2. Contact [ORE ID support](https://support.aikon.com) to register your dAPP for Custodial Functionality.  A service-key is required to call the ORE ID Custodial API.


## Running the sample app

1. Download this repo to your local file system.
2. Add your serviceKey to the "Assets/Scripts/APIConnection.cs" file.
3. Launch Unity Hub and select the "Open" option.
4. Choose the folder that contains the downloaded sample app.
5. Unity Hub will open a new window with the sample project.
6. Run the SampleScene by clicking the "Play" arrow.
7. Test the individual API Calls and see the feedback on the screen, additional info is printed to the console.


## The Unity script

Check the "Assets/Scripts" folder for the "APIConnection.cs" file. 

This APIConnection.cs file contains the functionality to call the ORE ID Service API.  The "CallApi" function is the manager which is attached to the API Manager Unity Object.  The function takes a string which triggers the corresponding API call.

Also included are data structures for the API requests and responses.


## Custodial Management

Custodial functionality for account creation, management, and signing keeps the control of the user's private keys with the developer.
This makes the onboarding experience of a new user straightforward.  The blockchain functionality can be hidden from the user entirely, creating a familiar gaming experience with no complicated blockchain wallet connection process.

Provisioning each of your game's users a blockchain account is the first step. Then, transactions can be conducted on the blockchain using the custodial blockchain accounts.

### Hiding blockchain functionality

Using Custodial tooling it is possible to mask blockchain functionality as familiar gaming actions.  Gamers are most-likely already comfortable with the ideas of in-game currency, digital marketplaces, and in-game purchases.  These are the most common actions that can be executed on the blockchain instead of silo'd in a centralized database.  Give gamers the right to own their digital assets.  

As users get more familiar with your game, you can slowly introduce them to different facets of the blockchain.  With the goal of giving full custody of the blockchain wallet to the user (after they have completed a certain task, racked up enough in game currency, or some other qualifying task).


## Expanding on this project

This project was created as a sample to demonstrate the ORE ID API Custodial functionality within the Unity environment.  This project does not present a working web wallet, but one can be built with the provided tooling.  Onboarding gamers quickly and outfitting them with a blockchain wallet and accounts is the goal of this project. Drop off rates are as high as 90% when gamers are presented with the action to link/sign-in their own crypto wallet.  Getting new gamers in the door and set up with a blockchain account with no effort on the gamer will ease onboarding drop off.

### Custodidal to Non-custodial migration

What if you want to transfer custodial user accounts to non-custodial?  This would give full custody of the wallet to the user.  Once converted to non-custodial, developers can no longer sign and broadcast transactions on behalf of the user.  Instead, the user will be asked to enter a passcode to sign the transaction.  This functionality is currently under construction, but will be available in a future release.