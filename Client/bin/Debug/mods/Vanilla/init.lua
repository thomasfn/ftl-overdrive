--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]
dofile( FOLDERNAME .. "/kestral.lua" )

local function GenerateEngi()
	local s = NewShip()
	s.Name = "The Kestral"
	s.BaseGraphic = "img/ship/circle_cruiser_base.png"
	s.CloakedGraphic = "img/ship/circle_cruiser_cloak.png"
	s.ShieldGraphic = "img/ship/circle_cruiser_shields.png"
	s.FloorGraphic = "img/ship/circle_cruiser_floor.png"
	s.GibGraphics:Add("img/ship/circle_cruiser_gib1.png")
	s.GibGraphics:Add("img/ship/circle_cruiser_gib2.png")
	s.GibGraphics:Add("img/ship/circle_cruiser_gib3.png")
	s.GibGraphics:Add("img/ship/circle_cruiser_gib4.png")
	s.GibGraphics:Add("img/ship/circle_cruiser_gib5.png")
	s.GibGraphics:Add("img/ship/circle_cruiser_gib6.png")
	s.TileHeight = 35
	s.TileWidth = 35
    s.FloorOffsetX = 69
    s.FloorOffsetY = 40
	--s.Weapons:Add("artemis")
	--s.Crew:Add("human_male")
	s:AddRectRoom(0, 0, 2, 2)
	--s.Rooms:Add(r)
	return s
end

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
	s.Order = 0
	
	s = library.AddSystem( "doorcontrol" )
	s.DisplayName = "Door Control"
	s.OverlayGraphic = "img/icons/s_doors_overlay.png"
	s.IconGraphics:Add( "img/icons/s_doors_red1.png" )
	s.IconGraphics:Add( "img/icons/s_doors_green1.png" )
	s.MinPower = 1
	s.MaxPower = 3
	s.Order = 1
	
	s = library.AddSystem( "sensors" )
	s.DisplayName = "Sensors"
	s.OverlayGraphic = "img/icons/s_sensors_overlay.png"
	s.IconGraphics:Add( "img/icons/s_sensors_red1.png" )
	s.IconGraphics:Add( "img/icons/s_sensors_green1.png" )
	s.MinPower = 1
	s.MaxPower = 3
	s.Order = 2
	
	s = library.AddSystem( "medbay" )
	s.DisplayName = "Medical Bay"
	s.OverlayGraphic = "img/icons/s_medbay_overlay.png"
	s.IconGraphics:Add( "img/icons/s_medbay_red1.png" )
	s.IconGraphics:Add( "img/icons/s_medbay_green1.png" )
	s.MinPower = 1
	s.MaxPower = 3
	s.Order = 3
	
	s = library.AddSystem( "o2" )
	s.DisplayName = "Oxygen Generator"
	s.OverlayGraphic = "img/icons/s_oxygen_overlay.png"
	s.IconGraphics:Add( "img/icons/s_oxygen_red1.png" )
	s.IconGraphics:Add( "img/icons/s_oxygen_green1.png" )
	s.MinPower = 1
	s.MaxPower = 3
	s.Order = 4
	
	s = library.AddSystem( "shields" )
	s.DisplayName = "Shields"
	s.OverlayGraphic = "img/icons/s_shields_overlay.png"
	s.IconGraphics:Add( "img/icons/s_shields_red1.png" )
	s.IconGraphics:Add( "img/icons/s_shields_green1.png" )
	s.MinPower = 2
	s.MaxPower = 8
	s.Order = 5
	
	s = library.AddSystem( "engines" )
	s.DisplayName = "Engines"
	s.OverlayGraphic = "img/icons/s_engines_overlay.png"
	s.IconGraphics:Add( "img/icons/s_engines_red1.png" )
	s.IconGraphics:Add( "img/icons/s_engines_green1.png" )
	s.MinPower = 2
	s.MaxPower = 6
	s.Order = 6
	
	s = library.AddSystem( "weapons" )
	s.DisplayName = "Weapons"
	s.OverlayGraphic = "img/icons/s_weapons_overlay.png"
	s.IconGraphics:Add( "img/icons/s_weapons_red1.png" )
	s.IconGraphics:Add( "img/icons/s_weapons_green1.png" )
	s.MinPower = 3
	s.MaxPower = 8
	s.Order = 7
	
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
	
	-- Add ship generators
	local sg = library.AddShipGenerator("kestral")
	sg.DisplayName = "The Kestral"
	sg.Unlocked = true
	sg.Default = true
	sg.NPC = false
	sg.MiniGraphic = "img/customizeUI/miniship_kestral.png"
	sg.Callback = Kestral
end

hook.Add( "Game.LoadLibrary", LoadLibrary )