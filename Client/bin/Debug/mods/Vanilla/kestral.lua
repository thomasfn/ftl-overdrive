--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]

function Kestral(s)
	-- Basic settings
	s.Name = "The Kestrel"
	
	-- Graphics
	s.BaseGraphic = "img/ship/kestral_base.png"
	s.CloakedGraphic = "img/ship/kestral_cloak.png"
	s.ShieldGraphic = "img/ship/kestral_shields.png"
	s.FloorGraphic = "img/ship/kestral_floor.png"
	s.GibGraphics:Add( "img/ship/kestral_gib1.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib2.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib3.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib4.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib5.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib6.png" )
	
	-- Floor settings
	s.TileHeight = 35
	s.TileWidth = 35
    s.FloorOffsetX = 71
    s.FloorOffsetY = 116
	
	-- Starter weapons
	--s.Weapons:Add( "artemis" )
	
	-- Starter crew
	--s.Crew:Add( "human_male" )
	
	-- Rooms
	s:AddRectRoom(0, 14, 2, 1, 2):SetSystem("bridge"):SetBackgroundGraphic("img/ship/interior/room_pilot.png")
	s:AddRectRoom(1, 12, 2, 2, 2)
	s:AddRectRoom(2, 10, 2, 2, 1):SetSystem("doorcontrol"):SetBackgroundGraphic("img/ship/interior/room_doors.png")
	s:AddRectRoom(3, 10, 3, 2, 1):SetSystem("sensors"):SetBackgroundGraphic("img/ship/interior/room_sensors.png")
	s:AddRectRoom(4, 8, 1, 2, 2):SetSystem("medbay"):SetBackgroundGraphic("img/ship/interior/room_medbay.png")
	s:AddRectRoom(5, 8, 3, 2, 2):SetSystem("shields"):SetBackgroundGraphic("img/ship/interior/room_shields.png")
	s:AddRectRoom(6, 6, 0, 2, 1)
	s:AddRectRoom(7, 6, 1, 2, 2)
	s:AddRectRoom(8, 6, 3, 2, 2)
	s:AddRectRoom(9, 6, 5, 2, 1)
	s:AddRectRoom(10, 4, 2, 2, 2):SetSystem("weapons"):SetBackgroundGraphic("img/ship/interior/room_weapons.png")
	s:AddRectRoom(11, 3, 1, 2, 1)
	s:AddRectRoom(12, 3, 4, 2, 1)
	s:AddRectRoom(13, 1, 1, 2, 1):SetSystem("o2"):SetBackgroundGraphic("img/ship/interior/room_oxygen.png")
	s:AddRectRoom(14, 1, 2, 2, 2):SetSystem("engines"):SetBackgroundGraphic("img/ship/interior/room_engines.png")
	s:AddRectRoom(15, 1, 4, 2, 1)
	s:AddRectRoom(16, 0, 2, 1, 2)
	
	-- Doors
	s.Doors:Add(ships.NewDoor({{Room = 1, X = 1, Y = 1, Dir = "Right"}, {Room = 0, X = 0, Y = 1, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 2, X = 1, Y = 0, Dir = "Right"}, {Room = 1, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 3, X = 1, Y = 0, Dir = "Right"}, {Room = 1, X = 0, Y = 1, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 4, X = 1, Y = 1, Dir = "Right"}, {Room = 2, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 5, X = 1, Y = 0, Dir = "Right"}, {Room = 3, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 4, X = 0, Y = 1, Dir = "Down"},
	                           {Room = 5, X = 0, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 7, X = 1, Y = 0, Dir = "Right"}, {Room = 4, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 8, X = 1, Y = 1, Dir = "Right"}, {Room = 5, X = 0, Y = 1, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = -1, X = 0, Y = 0, Dir = "Down"}, -- outside
	                              {Room = 6, X = 1, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = -1, X = 0, Y = 0, Dir = "Down"}, -- outside
	                           {Room = 6, X = 0, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 6, X = 1, Y = 0, Dir = "Down"},
	                           {Room = 7, X = 1, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 8, X = 1, Y = 1, Dir = "Down"},
	                           {Room = 9, X = 1, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 9, X = 1, Y = 0, Dir = "Down"},
	                           {Room = -1, X = 0, Y = 0, Dir = "Up"}})); -- outside
	
	s.Doors:Add(ships.NewDoor({{Room = 9, X = 0, Y = 0, Dir = "Down"},
	                           {Room = -1, X = 0, Y = 0, Dir = "Up"}})); -- outside
	
	s.Doors:Add(ships.NewDoor({{Room = 10, X = 1, Y = 0, Dir = "Right"}, {Room = 7, X = 0, Y = 1, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 10, X = 1, Y = 1, Dir = "Right"}, {Room = 8, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 11, X = 1, Y = 0, Dir = "Down"},
	                           {Room = 10, X = 0, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 10, X = 0, Y = 1, Dir = "Down"},
	                           {Room = 12, X = 1, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 13, X = 1, Y = 0, Dir = "Right"}, {Room = 11, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 15, X = 1, Y = 0, Dir = "Right"}, {Room = 12, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 13, X = 1, Y = 0, Dir = "Down"},
	                           {Room = 14, X = 1, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 14, X = 1, Y = 1, Dir = "Down"},
	                           {Room = 15, X = 1, Y = 0, Dir = "Up"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 16, X = 0, Y = 0, Dir = "Right"}, {Room = 14, X = 0, Y = 0, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = 16, X = 0, Y = 1, Dir = "Right"}, {Room = 14, X = 0, Y = 1, Dir = "Left"}}));
	
	s.Doors:Add(ships.NewDoor({{Room = -1, X = 0, Y = 0, Dir = "Right"}, {Room = 16, X = 0, Y = 0, Dir = "Left"}})); -- outside
	
	s.Doors:Add(ships.NewDoor({{Room = -1, X = 0, Y = 0, Dir = "Right"}, {Room = 16, X = 0, Y = 1, Dir = "Left"}})); -- outside
	return s
end
