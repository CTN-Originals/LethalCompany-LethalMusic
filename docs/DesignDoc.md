`?` = Either/Or (default if not specified)
`#` = Collection
`!` = Required
`*` = Exception
`(?)` = Concept/Idea

## Monster Interaction
- [ ] `#` **Encounter**
	- [ ] `!` Player detects mob
		- [ ] `!` Mob is within a certain distance
		- [ ] `?` Mob can be seen by the player
			- [ ] `!` Line of sight
			- [ ] `(?)` Light level
	- [ ] `?` Mob is withing a certain distance of the player
		- [ ] `!` Distance mesured by unobstructed path to the player
	- [ ] `?` Mob detects player
	- [ ] `!` Mob is hostile
		- [ ] `*` Mimic
			- [ ] `?` Stops acting like a player
			- [ ] `?` Player put on the mimic mask and is taken over
		- [ ] `*` Friendly Dog
			- [ ] `?` Gets startled
- [ ] `#` **Attack**
	- [ ] `!` Encounter has occurred
	- [ ] `!` Mob is hostile
	- [ ] `?` Mob chases the player
	- [ ] `?` Mob is searching for the player
	- [ ] `?` Mob attacks the player
- [ ] `#` **End**
	- [ ] `!` Attack in progress
	- [ ] `?` Escaped
		- [ ] `?` Player is out of sight
		- [ ] `?` Player is out of range
		- [ ] `?` Mob gives up
	- [ ] `?` Player dies
		- [ ] `?` Killed by the mob
		- [ ] `?` By other means
	- [ ] `?` Mob is killed
		- [ ] `?` By the player
		- [ ] `?` By other means

## Custom Monster user entry
- [ ] `(?)` **JSON user entry system**
	- [ ] `!` User can input a JSON file to define a custom monster's behavior