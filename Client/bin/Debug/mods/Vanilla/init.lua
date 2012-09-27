--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]

local function LoadLibrary()
	-- Notify console
	print( "[Vanilla] Loading content..." )
	
	-- Add weapon types
	local w = library.AddWeapon( "artemis", "projectile" )
	w.DisplayName = "Artemis"
	w.Graphic = "img/weapons/missiles_1_strip3.png"
	w.GlowGraphic = "img/weapons/missiles_1_glow.png"
	w.ProjectileGraphic = "img/weapons/missile_1.png"
	w.PowerCost = 1
	w.BypassShield = true
	w.Damage = 1
	w.HitChance = 1
	
	-- Add system types
	local s = library.AddSystem( "bridge" )
	s.DisplayName = "Bridge"
	s.OverlayGraphic = "img/icons/s_pilot_overlay.png"
	s.IconGraphics:Add( "img/icons/s_pilot_red1.png" )
	s.IconGraphics:Add( "img/icons/s_pilot_green1.png" )
	--s.SubSystem = true -- This crashes for some reason?
	s.MinPower = 1
	s.MaxPower = 3
	
	-- Add races
	local r = library.AddRace( "human_male" )
	r.DisplayName = "Human"
	r.Names:Add( "Bob" )
	r.Names:Add( "Bill" )
	r.TilesheetGreen = "img/people/human_player_green.png"
	r.TilesheetRed = "img/people/human_enemy_red.png"
	r.TilesheetYellow = "img/people/human_player_yellow.png"
	r.TilesheetSelected = "img/people/human_player_highlight.png"
	r.TilesX = 16
	r.TilesY = 9
	r.Animations:Add( "walk.down", library.CreateAnimation( 1, 4, 1 ) )
	r.Animations:Add( "walk.right", library.CreateAnimation( 5, 8, 1 ) )
	r.Animations:Add( "walk.up", library.CreateAnimation( 9, 12, 1 ) )
	r.Animations:Add( "walk.left", library.CreateAnimation( 13, 16, 1 ) )
	-- todo: the rest of the animations
	
	-- Add ships
	local s = library.AddShip( "kestral" )
	s.DisplayName = "The Kestral"
	s.Unlocked = true
	s.Default = true
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
	s.Weapons:Add( "artemis" )
	s.Crew:Add( "human_male" )
	local r = library.CreateRoom( 1, 1, 2, 1 )
	r.BackgroundGraphic = "img/ship/interior/room_oxygen_2.png"
	r.Doors:Add( library.CreateDoor( 0.5, 1 ) )
	r.Doors:Add( library.CreateDoor( 1.5, 1 ) )
	s.Rooms:Add( r )
	
	s = library.AddShip( "engi" )
	s.DisplayName = "Engi Cruiser"
	s.Unlocked = true
	s.Default = false
	s.BaseGraphic = "img/ship/circle_cruiser_base.png"
	s.CloakedGraphic = "img/ship/circle_cruiser_cloak.png"
	s.ShieldGraphic = "img/ship/circle_cruiser_shields.png"
	s.FloorGraphic = "img/ship/circle_cruiser_floor.png"
	s.MiniGraphic = "img/customizeUI/miniship_circle_cruiser.png"
	
	s = library.AddShip( "rock" )
	s.DisplayName = "Rock Cruiser"
	s.Unlocked = true
	s.Default = false
	s.BaseGraphic = "img/ship/rock_cruiser_base.png"
	s.CloakedGraphic = "img/ship/rock_cruiser_cloak.png"
	s.ShieldGraphic = "img/ship/rock_cruiser_shields.png"
	s.FloorGraphic = "img/ship/rock_cruiser_floor.png"
	s.MiniGraphic = "img/customizeUI/miniship_rock_cruiser.png"
	
	s = library.AddShip( "fed" )
	s.DisplayName = "Federation Cruiser"
	s.Unlocked = false
	s.Default = false
	s.BaseGraphic = "img/ship/fed_cruiser_base.png"
	s.CloakedGraphic = "img/ship/fed_cruiser_cloak.png"
	s.ShieldGraphic = "img/ship/fed_cruiser_shields.png"
	s.FloorGraphic = "img/ship/fed_cruiser_floor.png"
	s.MiniGraphic = "img/customizeUI/miniship_fed_cruiser.png"
end
hook.Add( "Game.LoadLibrary", LoadLibrary )