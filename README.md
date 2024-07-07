# Rangers
This is a fun project where we have tank rangers of various abilities who can fight among each other.

# Rangers
- Red ranger
- Blue ranger
- Yellow ranger
- Green ranger

# Primary Actions
- Tesla Ball
- Projectile

# Ultimate Actions
- AirStrike
- Self Shield
- Invisibility
- Auto Target

# Design Pattern used

  * Scriptable Objects use cases
    - Used for creating Factory classes so that they can easily be referenced in the editor for DI.
    - Data Containers for Flyweight Pattern
    - Observable Generic Data Containers - if the data inside the scriptable object changes, it can invoke events. [GenericDataSO etc]
    - Extendable Enums - Helpful to get rid of enums, and a class to know all types of enums for fetching the item. [TagSO etc]
    - Generic Event Channels - To facilitate observer pattern. [GenericEventChannelSO etc.]
    
- Flyweight
- Dependency Injection using VContainer [ previously created own lightweight Dependency Injection Framework]
- Model-view-controller (entities, player, enemy, actions)
- Observer pattern (Event Channel, EventBus)
- Generic Factory pattern (EntityFactories, RagdollFactories, PrimaryActionFactories, UltimateActionFactories, EffectsFactories) 
- Dynamic Generic Object Pooling (entities, effects, actions)
- Generic State Machine (enemy state, game state)
- Builder (to build entities, player, enemy etc)

# Sceeenshots
Tank Selection Panel
 ![TankSelection UI](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/ea05850b-1e8e-4044-bd5b-857b08c8c587)


Ultimate Actions Abilities
- Air Strike
 ![AirStrike](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/637e3870-e2c9-4432-bb95-d59dbedb8d4c)

 Flow Diagram
 ![AirStrike Flow Diagram](https://github.com/YsKhan61/Rangers/assets/30847550/54d38e54-a5dc-48ab-ae66-c37a532cd988)


- Self Shield
 ![SelfShield](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/a9c84398-d117-4f99-88b9-e02f4843dc2d)

 Flow Diagram
 ![Self shield Flow Diagram](https://github.com/YsKhan61/Rangers/assets/30847550/29132bae-584e-4519-88e2-2b8863499140)


- Invisibility
 ![Invisibility Ultimate](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/00a2e16d-6d8a-4c3c-a724-e54d6f74dcee)

 Flow Diagram
 ![Invisibility Flow Diagram](https://github.com/YsKhan61/Rangers/assets/30847550/358ed2b5-dc6b-4c8f-9a74-53c513284b86)


- Auto Target
 ![AutoTarget](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/88ce33d5-a54f-4ea6-a968-bf167c9e2221)

 Flow Diagram
 ![AutoTarget Flow Diagram](https://github.com/YsKhan61/Rangers/assets/30847550/1ad0dcd9-283c-46b3-9fb1-5452ac7ce490)


Primary Actions Abilities
- Charged Firing
 ![ChargedFiring Screenshot](https://github.com/YsKhan61/Rangers/assets/30847550/7fcee606-7bf6-46f9-abdc-38dfa5eb8a58)

 Flow Diagram
 ![ChargedFiring Flow Diagram](https://github.com/YsKhan61/Rangers/assets/30847550/db5d5987-9e37-4443-830e-cece494db884)

  
- Tesla Firing
 ![TeslaFiring ScreenShot](https://github.com/YsKhan61/Rangers/assets/30847550/147af523-bccb-45e2-abf5-4b18132c2bde)

 Flow Diagram
 ![Tesla Firing Flow Diagram](https://github.com/YsKhan61/Rangers/assets/30847550/6b98d9c5-b749-4a44-89de-3bdde005debf)
