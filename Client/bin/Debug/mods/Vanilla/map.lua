--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]

local function WeightedRandom(weights)
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

function GenerateDefaultSectorMap(sm)
	math.randomseed(os.time())
	local sectorTypes = {
						{Type = "Civilian", Name = "Civilian Sector",          MinSector = 1},
						{Type = "Civilian", Name = "Engi Controlled Sector",   MinSector = 1},
						{Type = "Civilian", Name = "Engi Homeworlds",          MinSector = 3},
						{Type = "Civilian", Name = "Zoltan Controlled Sector", MinSector = 2},
						{Type = "Civilian", Name = "Zoltan Homeworlds",        MinSector = 3},
						
						{Type = "Hostile",  Name = "Mantis Controlled Sector", MinSector = 1},
						{Type = "Hostile",  Name = "Mantis Homeworlds",        MinSector = 3},
						{Type = "Hostile",  Name = "Pirate Controlled Sector", MinSector = 1},
						{Type = "Hostile",  Name = "Rebel Controlled Sector",  MinSector = 1},
						{Type = "Hostile",  Name = "Rock Controlled Sector",   MinSector = 2},
						{Type = "Hostile",  Name = "Rock Homeworlds",          MinSector = 1},
						
						{Type = "Nebula",   Name = "Slug Controlled Nebula",   MinSector = 4},
						{Type = "Nebula",   Name = "Slug Home Nebula",         MinSector = 4},
						{Type = "Nebula",   Name = "Uncharted Nebula",         MinSector = 4},
						}
	local sectorTypeWeights = {
							1/5, 1/5, 1/5, 1/5, 1/5,
							1/6, 1/6, 1/6, 1/6, 1/6, 1/6,
							1/3, 1/3, 1/3,
						}
	
	-- Calculate number of nodes per column
	local nNodes = {}
	nNodes[1] = 1;
	nNodes[2] = 2;
	for i = 3,6 do
		nNodes[i] = math.random(2, 4)
	end
	nNodes[7] = 2;
	nNodes[8] = 1;
	
	-- Generate nodes and add them to sector map
	local nodes = {}
	for i = 1,8 do nodes[i] = {} end
	
	nodes[1][1] = sm:AddNode("Civilian", "You should never see this text", 35, 74)
	for i = 2,7 do
		for j = 1,nNodes[i] do
			local k;
			repeat
				k = WeightedRandom(sectorWeights)
			until sectorTypes[k].MinSector <= i
			nodes[i][j] = sm:AddNode(sectorTypes[k].Type, sectorTypes[k].Name, 70 * i - 35, 35 * (j - (nNodes[i] + 1) / 2) + 74)
		end
	end
	nodes[8][1] = sm:AddNode("Hostile", "The Last Stand", 70 * 8 - 35, 74)
	
	-- Add connections
	nodes[1][1].NextNodes:Add(nodes[2][1])
	nodes[1][1].NextNodes:Add(nodes[2][2])
	for i = 2,6 do
		if nNodes[i] == 2 then
			if nNodes[i + 1] == 2 then
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
			if nNodes[i + 1] == 2 then
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
			if nNodes[i + 1] == 2 then
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
	nodes[7][1].NextNodes:Add(nodes[8][1])
	nodes[7][2].NextNodes:Add(nodes[8][1])
	
	return sm
end