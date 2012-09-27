--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]

local function Kestral()
	-- Basic settings
	local s = library.AddShip( "kestral" )
	s.DisplayName = "The Kestral"
	s.Unlocked = true
	s.Default = true
	
	-- Graphics
	s.BaseGraphic = "img/ship/kestral_base.png"
	s.CloakedGraphic = "img/ship/kestral_cloak.png"
	s.ShieldGraphic = "img/ship/kestral_shields.png"
	s.FloorGraphic = "img/ship/kestral_floor.png"
	s.MiniGraphic = "img/customizeUI/miniship_kestral.png"
	s.GibGraphics:Add( "img/ship/kestral_gib1.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib2.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib3.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib4.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib5.png" )
	s.GibGraphics:Add( "img/ship/kestral_gib6.png" )
	
	-- Starter weapons
	s.Weapons:Add( "artemis" )
	
	-- Starter crew
	s.Crew:Add( "human_male" )
	
	-- Rooms
	local r = library.CreateRoom( 2, 2, 3, 2 )
	r.System = "o2"
	r.BackgroundGraphic = "img/ship/interior/room_oxygen_2.png"
	r.Doors:Add( library.CreateDoor( 2, 3, "down" ) )
	r.Doors:Add( library.CreateDoor( 2, 3, "right" ) )
	s.Rooms:Add( r )
	
	r = library.CreateRoom( 4, 2, 5, 2 )
	r.Doors:Add( library.CreateDoor( 4, 2, "left" ) )
	r.Doors:Add( library.CreateDoor( 5, 2, "down" ) )
	s.Rooms:Add( r )
	
	r = library.CreateRoom( 2, 3, 3, 4 )
	r.System = "engines"
	r.Doors:Add( library.CreateDoor( 2, 3, "left" ) )
	r.Doors:Add( library.CreateDoor( 2, 4, "left" ) )
	r.Doors:Add( library.CreateDoor( 3, 3, "up" ) )
	r.Doors:Add( library.CreateDoor( 3, 4, "down" ) )
	s.Rooms:Add( r )
	
	r = library.CreateRoom( 1, 3, 1, 4 )
	r.Doors:Add( library.CreateDoor( 1, 3, "left" ) )
	r.Doors:Add( library.CreateDoor( 1, 4, "left" ) )
	r.Doors:Add( library.CreateDoor( 1, 3, "right" ) )
	r.Doors:Add( library.CreateDoor( 1, 4, "right" ) )
	s.Rooms:Add( r )
end
hook.Add( "Game.LoadLibrary", Kestral )