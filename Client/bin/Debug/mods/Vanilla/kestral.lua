--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]

function Kestral()
	-- Basic settings
	local s = NewShip()
	s.Name = "The Kestral"
	
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
	s:AddRectRoom(0, 14, 2, 1, 2)
	s:AddRectRoom(1, 12, 2, 2, 2)
	s:AddRectRoom(2, 10, 2, 2, 1)
	s:AddRectRoom(3, 10, 3, 2, 1)
	s:AddRectRoom(4, 8, 1, 2, 2)
	s:AddRectRoom(5, 8, 3, 2, 2)
	s:AddRectRoom(6, 6, 0, 2, 1)
	s:AddRectRoom(7, 6, 1, 2, 2)
	s:AddRectRoom(8, 6, 3, 2, 2)
	s:AddRectRoom(9, 6, 5, 2, 1)
	s:AddRectRoom(10, 4, 2, 2, 2):SetSystem("weapons"):SetBackgroundGraphic("img/ship/interior/room_weapons.png")
	s:AddRectRoom(11, 3, 1, 2, 1)
	s:AddRectRoom(12, 3, 4, 2, 1)
	s:AddRectRoom(13, 1, 1, 2, 1)
	s:AddRectRoom(14, 1, 2, 2, 2)
	s:AddRectRoom(15, 1, 4, 2, 1)
	s:AddRectRoom(16, 0, 2, 1, 2)
	return s
end