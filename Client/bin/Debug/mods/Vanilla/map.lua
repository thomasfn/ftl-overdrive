--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]
local Map = CLRPackage("FTLOverdriveClient", "FTLOverdrive.Client.Map")
local SectorMap = Map.SectorMap



function generateVanillaSectorMap()
	math.randomseed(os.time())
	-- skip a few first numbers
	for i = 1,10 do	math.random() end
	
	local firstSector = stdmap.generateNormalSector(1, stdmap.CIVILIAN_COLOR, "Civilian Sector")
	local sectorTypes = {
						{minLevel = 1, weight = 1/5, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.CIVILIAN_COLOR, "Civilian Sector") end},
						{minLevel = 1, weight = 1/5, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.CIVILIAN_COLOR, "Engi Controlled Sector") end},
						{minLevel = 3, weight = 1/5, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.CIVILIAN_COLOR, "Engi Homeworlds Sector") end},
						{minLevel = 2, weight = 1/5, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.CIVILIAN_COLOR, "Zoltan Controlled Sector") end},
						{minLevel = 3, weight = 1/5, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.CIVILIAN_COLOR, "Zoltan Homeworlds") end},
						
						{minLevel = 1, weight = 1/6, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Mantis Controlled Sector") end},
						{minLevel = 3, weight = 1/6, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Mantis Homeworlds") end},
						{minLevel = 1, weight = 1/6, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Pirate Controlled Sector") end},
						{minLevel = 1, weight = 1/6, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Rebel Controlled Sector") end},
						{minLevel = 2, weight = 1/6, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Rock Controlled Sector") end},
						{minLevel = 1, weight = 1/6, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Rock Homeworlds") end},
						
						{minLevel = 4, weight = 1/3, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.NEBULA_COLOR, "Slug Controlled Nebula") end},
						{minLevel = 4, weight = 1/3, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.NEBULA_COLOR, "Slug Controlled Nebula") end},
						{minLevel = 1, weight = 1/3, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.NEBULA_COLOR, "Uncharted Nebula") end},
						}
	local lastSector = stdmap.generateNormalSector(8, stdmap.HOSTILE_COLOR, "The Last Stand")
	
	return stdmap.GenerateSectorMap(firstSector, sectorTypes, lastSector, {0, 1, 2, 1}, 0.4)
end