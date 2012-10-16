--[[
	Package: stdmap
	
	Standard functions for map generation.
]]
stdmap = {}

local Graphics = CLRPackage("sfmlnet-graphics-2.dll", "SFML.Graphics")
local Color = Graphics.Color
local Map = CLRPackage("FTLOverdriveClient", "FTLOverdrive.Client.Map")
local SectorMap = Map.SectorMap
local Sector = Map.Sector
local Beacon = Map.Beacon

local function weightedRandom(weights)
	local sum = 0
	for i = 1,#weights do
		sum = sum + weights[i]
	end
	local r = math.random() * sum
	sum = 0
	for i = 1,#weights do
		sum = sum + weights[i]
		if sum > r then return i end
	end
	return 1
end

--[[
	Constants: Sector Colors
	
	CIVILIAN_COLOR - Green (135, 200, 75)
	HOSTILE_COLOR  - Red (215, 50, 50)
	NEBULA_COLOR   - Purple (128, 50, 210)
]]
stdmap.CIVILIAN_COLOR = Color(135, 200, 75)
stdmap.HOSTILE_COLOR  = Color(215, 50, 50)
stdmap.NEBULA_COLOR   = Color(128, 50, 210)

--[[
	Function: generateNormalSector
	
	Generates a normal sector.
	
	Parameters:
		difficulty    - Sector difficulty. When used with <GenerateSectorMap>,
						it is equal to column in which this sector is generated
						and ranges from 1 to 8.
		color         - Color that this sector will have on the sector map.
        name          - Sector name.
        nBeacons      - Number of beacons. Can't be larger than 24 * 16 = 384.
						Default: random between 20 and 25
	
	Returns:
		Generated sector.
	
	Example:
		(start code)
		local firstSector = stdmap.generateNormalSector(1, stdmap.CIVILIAN_COLOR, "Starting Sector")
		(end code)
]]

function stdmap.generateNormalSector(difficulty, color, name, nBeacons)
	nBeacons = nBeacons or math.random(20, 25)
	if nBeacons > 24 * 16 then nBeacons = 24 * 16 end
	
	local s = Sector(name, color)
	
	-- Chose nBeacons different random pairs of integers.
	local pos = {}
	for i = 1,nBeacons do
		repeat
			pos[i] = {math.random(24), math.random(16)}
			local unique = true
			for j = 1,i-1 do
				if pos[i][1] == pos[j][1] and pos[i][2] == pos[j][2] then unique = false end
			end
		until unique
	end
	
	-- Place beacons near those locations (with random offsets to make grid less noticable for the player)
	for i = 1,nBeacons do
		local b = Beacon(20 + 20 * pos[i][1] + math.random(10) - 5, 50 + 20 * pos[i][2] + math.random(10) - 5)
		s.Beacons:Add(b)
	end
	return s
end


--[[
	Function: GenerateSectorMap
	
	Generates a classical-looking sector map. It can have custom sectors
	types, or custom number of sectors per column.
	
	Parameters:
		firstSector           - Starting sector.
		sectorTypes           - Array of sector types. Each element is a table (see below).
								This is used for all sectors except first one and last one.
		lastSector            - Final sector.
		probSectorsPerColumn  - Array of probabilities* of getting specific number
								of sectors in a column. Default: {0, 1, 2, 1},
		probMultiplier        - Probability* to get the same number of sectors as in
								previous column is multiplied by this value. Default: 0.5.
	
	Sector type format:
		Each element of sectorTypes array is a table containing following values:
		
		minLevel          - Minimal difficulty level (1..8).
		weight            - Probability* of choosing this type.
		generateSector    - Function that will generate the sector,
							takes difficulty level and returns Sector.
	
	Returns:
		Generated sector map.
	
	Probability:
		[*]If sum of probabilities is not 1, all of them are scaled so that the sum becomes 1.
	
	Example:
		(start code)
		local firstSector = stdmap.generateNormalSector(1, stdmap.CIVILIAN_COLOR, "Starting Sector")
		local sectorTypes = {
							{minLevel = 1, weight = 1, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.CIVILIAN_COLOR, "Civilian Sector") end},
							{minLevel = 1, weight = 1/2, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Mantis Controlled Sector") end},
							{minLevel = 3, weight = 1/2, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.HOSTILE_COLOR, "Mantis Homeworlds") end},
							{minLevel = 1, weight = 1, generateSector = function(difficulty) return stdmap.generateNormalSector(difficulty, stdmap.NEBULA_COLOR, "Uncharted Nebula") end},
							}
		local lastSector = stdmap.generateNormalSector(8, stdmap.HOSTILE_COLOR, "The Last Stand")

		local sectorMap = stdmap.GenerateSectorMap(firstSector, sectorTypes, lastSector, {0, 1, 1, 0})
		(end code)
]]
function stdmap.GenerateSectorMap(firstSector, sectorTypes, lastSector, probSectorsPerColumn, probMultiplier)
	probSectorsPerColumn = probSectorsPerColumn or {0, 1, 2, 1}
	probMultiplier = probMultiplier or 0.5
	local sm = SectorMap()
	
	-- Calculate number of sectors per column
	local nNodes = {}
	nNodes[1] = 1;
	nNodes[2] = 2;
	for i = 3,7 do
		local prob = probSectorsPerColumn
		prob[nNodes[i - 1]] = prob[nNodes[i - 1]] * probMultiplier
		nNodes[i] = weightedRandom(prob)
	end
	nNodes[8] = 1;
	
	-- Generate sectors and add nodes to sector map
	local nodes = {}
	for i = 1,8 do nodes[i] = {} end
	
	nodes[1][1] = sm:AddNode(35, 74, firstSector)
	for i = 2,7 do
		for j = 1,nNodes[i] do
			local prob = {}
			for k = 1,#sectorTypes do
				if sectorTypes[k].minLevel <= i then
					prob[k] = sectorTypes[k].weight
				else prob[k] = 0
				end
			end
			local sector = sectorTypes[weightedRandom(prob)].generateSector(i)
			nodes[i][j] = sm:AddNode(70 * i - 35, 35 * (j - (nNodes[i] + 1) / 2) + 74, sector)
		end
	end
	nodes[8][1] = sm:AddNode(70 * 8 - 35, 74, lastSector)
	
	-- Add connections
	for i = 1,7 do
		-- maybe this code can be made a lot cleaner?
		if nNodes[i] == 1 then
			if nNodes[i + 1] == 1 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
			elseif nNodes[i + 1] == 2 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][1].NextNodes:Add(nodes[i + 1][2])
			elseif nNodes[i + 1] == 3 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][1].NextNodes:Add(nodes[i + 1][2])
				nodes[i][1].NextNodes:Add(nodes[i + 1][3])
			elseif nNodes[i + 1] == 4 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][1].NextNodes:Add(nodes[i + 1][2])
				nodes[i][1].NextNodes:Add(nodes[i + 1][3])
				nodes[i][1].NextNodes:Add(nodes[i + 1][4])
			end
		elseif nNodes[i] == 2 then
			if nNodes[i + 1] == 1 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][1])
			elseif nNodes[i + 1] == 2 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
			elseif nNodes[i + 1] == 3 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][1].NextNodes:Add(nodes[i + 1][2])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
				nodes[i][2].NextNodes:Add(nodes[i + 1][3])
			elseif nNodes[i + 1] == 4 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][1].NextNodes:Add(nodes[i + 1][2])
				nodes[i][2].NextNodes:Add(nodes[i + 1][3])
				nodes[i][2].NextNodes:Add(nodes[i + 1][4])
			end
		elseif nNodes[i] == 3 then
			if nNodes[i + 1] == 1 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][1])
				nodes[i][3].NextNodes:Add(nodes[i + 1][1])
			elseif nNodes[i + 1] == 2 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
				nodes[i][3].NextNodes:Add(nodes[i + 1][2])
			elseif nNodes[i + 1] == 3 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
				nodes[i][3].NextNodes:Add(nodes[i + 1][3])
			elseif nNodes[i + 1] == 4 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][1].NextNodes:Add(nodes[i + 1][2])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
				nodes[i][2].NextNodes:Add(nodes[i + 1][3])
				nodes[i][3].NextNodes:Add(nodes[i + 1][3])
				nodes[i][3].NextNodes:Add(nodes[i + 1][4])
			end
		elseif nNodes[i] == 4 then
			if nNodes[i + 1] == 1 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][1])
				nodes[i][3].NextNodes:Add(nodes[i + 1][1])
				nodes[i][4].NextNodes:Add(nodes[i + 1][1])
			elseif nNodes[i + 1] == 2 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][1])
				nodes[i][3].NextNodes:Add(nodes[i + 1][2])
				nodes[i][4].NextNodes:Add(nodes[i + 1][2])
			elseif nNodes[i + 1] == 3 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
				nodes[i][3].NextNodes:Add(nodes[i + 1][2])
				nodes[i][3].NextNodes:Add(nodes[i + 1][3])
				nodes[i][4].NextNodes:Add(nodes[i + 1][3])
			elseif nNodes[i + 1] == 4 then
				nodes[i][1].NextNodes:Add(nodes[i + 1][1])
				nodes[i][2].NextNodes:Add(nodes[i + 1][2])
				nodes[i][3].NextNodes:Add(nodes[i + 1][3])
				nodes[i][4].NextNodes:Add(nodes[i + 1][4])
			end
		end
	end
	
	return sm
end