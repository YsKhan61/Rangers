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

Ultimate Abilities
- Air Strike
![AirStrike](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/637e3870-e2c9-4432-bb95-d59dbedb8d4c)

- Self Shield
![SelfShield](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/a9c84398-d117-4f99-88b9-e02f4843dc2d)

- Invisibility
![Invisibility Ultimate](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/00a2e16d-6d8a-4c3c-a724-e54d6f74dcee)

- Auto Target
![AutoTarget](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/88ce33d5-a54f-4ea6-a968-bf167c9e2221)

Flow Diagram
![Ultimate System Flow Diagram](https://github.com/YsKhan61/Tank_Rangers/assets/30847550/c70ae924-27f8-440d-afa1-3c8b7f4848ee)
