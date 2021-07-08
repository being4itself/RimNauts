# RimNauts

---

## Bugs

- SOS2 replaces vacuum tile which makes it look like an ocean (blue) / seems to add rock roofs in vaccum

  - This does not happen without SOS2
	- We can patch a quick fix for SoS2.

- Still adds other types of stone (should just be moon rock)

  - Got no moon stone on the moon, almost all of the home planet had moonstone
	- We have been working on this. the moon biome should spawn only moonstone and basalt. moonstone and basalt will have special chunk shapes. however, the chunks from the original tile are still made of other minerals. swapping chunks out gave us problems, but having chunks of random stuff up there isn't that bad. moonstone and basalt chunks will need to be mined.
	
- The texture for the minerals are not properly switching. the swapped minerals still look like the original mineral that is being replaced even though it clearly says that it is the new mineral. when you mine the mineral then it and all of the surrounding ground and rock will switch to the correct texture. we are trying to figure something out.

## Roadmap

- [x] Orbiting moon

- [x] Playable moon biome

  - [x] Lunar terrain

  - [x] Disable any spawning events (drastically increase performance)

  - [x] Disable all walking caravan/raiders events (replace with orbital trading and drop pod raids)

  - [x] Drasticly increase/decrease temperature depending on night and day cycle

  - Harvesting resources

   - [ ] Mining vessels operated by colonist traveling to "asteroids" to mine random minerals and chunks (can just be behind the scenes like the empire mod - good for performance)

   - [ ] Random anomalies that the user can decide what they want to do with. Random outcomes (like text-based RPG games)

   - [ ] A ship capable landing on the Rimworld to gather resources

   - [ ] (Late game) AI Ships capable of mining and hauling (from one planet to another) without colonist operating

- [ ] Add oxygen variable (Maybe depend on sos2)

- [ ] Add functioning solar system
