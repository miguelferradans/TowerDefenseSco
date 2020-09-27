# Tower Defense Project

## Unity Version Used

- 2019.4.0f1

## How To Play

Open LoadScene and press Play.

## Deliverables

1. Done
2. Done
3. Done
4. Done
5. Done
6. Done
7. Not Done
8. Done
9. Done
10. Not Done
11. Done

## LogBook

### Project Design

Scalable design where in-disk data, runtime state and unity components are separated and isolated from each other.

- All the game configuration is layed out in read-only ScriptableObjects, Levels, Level List, Enemies, Towers, etc.
- Runtime representations of these ScriptableObjects are created within the scripts in Logic folder.
- Unity components are present in the MonoBehaviour component.
- Implementing new design elements such as new towers or enemies doesn't take long.
- Levels are self-contained, loading a Level into memory has all the necessary dependencies to work.
- Created a rudimentary Level Editor, to be able to quickly make new levels and iterate over them.

### TimeLine

### Friday

- Decided on the architecture and started implementing the different base ScriptableObjects
- Implemented a rudimentary EditorWindow to be able to customize Levels

### Saturday

- Focused on finishing up the ScriptableObjects and implementing the different logic classes needed
- Created the basic scenes setup, cameras, Canvas for UI, etc
- Finished the day implementing the MonoBehaviours that would link up the data and logic with Unity itself

### Sunday

- Implemented a couple of different towers, enemies and designed a few levels to test
- Polished scenes and general flow of the game, tested everything, didn't really encounter any bugs that slowed my progression

I spent zero time looking for any asset to make the game look prettier, I considered it a waste of time, since thats not what should be revised here.

### Concessions made on the design because of time constraints

- Dependency injection is very rudimentary, its mostly done through the LevelComponents object.
- My initial idea was to separate logic and Unity visual components completely, however, since this is not a trivial task, and requires fine-tuning to achieve proper interpolation between logic and visual states, I decided against it, and some Unity components do logic-related tasks using delta time.
- All the settings are tied to the Level ScriptableObject, which doesn't allow to have general settings that will be shared amongst all levels.
- Level Editor is missing quite a lot of basic features, like Undo support or shift clicking slots, but for the time spent, it works.
- Some UI classes like the Buy buttons MonoBehaviours could use a better base class since some code is repeated.
- Pooling, obviously, I decided against implementing a basic pooling system for enemies and projectiles, since this is very basic and everyone knows how to do it.