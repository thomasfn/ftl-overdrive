--[[
	****************************
	* FTL: Overdrive - Vanilla *
	****************************
	Loads the vanilla content into the game
--]]

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

local function GenerateKestral()
	local s = NewShip()
	s.Name = "The Kestral"
	s.BaseGraphic = "img/ship/kestral_base.png"
	s.CloakedGraphic = "img/ship/kestral_cloak.png"
	s.ShieldGraphic = "img/ship/kestral_shields.png"
	s.FloorGraphic = "img/ship/kestral_floor.png"
	s.GibGraphics:Add("img/ship/kestral_gib1.png")
	s.GibGraphics:Add("img/ship/kestral_gib2.png")
	s.GibGraphics:Add("img/ship/kestral_gib3.png")
	s.GibGraphics:Add("img/ship/kestral_gib4.png")
	s.GibGraphics:Add("img/ship/kestral_gib5.png")
	s.GibGraphics:Add("img/ship/kestral_gib6.png")
	s.TileHeight = 35
	s.TileWidth = 35
    s.FloorOffsetX = 71
    s.FloorOffsetY = 116
	--s.Weapons:Add("artemis")
	--s.Crew:Add("human_male")
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
	s:AddRectRoom(10, 4, 2, 2, 2)
	s:AddRectRoom(11, 3, 1, 2, 1)
	s:AddRectRoom(12, 3, 4, 2, 1)
	s:AddRectRoom(13, 1, 1, 2, 1)
	s:AddRectRoom(14, 1, 2, 2, 2)
	s:AddRectRoom(15, 1, 4, 2, 1)
	s:AddRectRoom(16, 0, 2, 1, 2)
	s.Rooms[10].BackgroundGraphic = "img/ship/interior/room_weapons.png"
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
	sg.Callback = GenerateKestral
	
	sg = library.AddShipGenerator("engi")
	sg.DisplayName = "The Torus"
	sg.Unlocked = true
	sg.Default = false
	sg.NPC = false
	sg.MiniGraphic = "img/customizeUI/miniship_circle_cruiser.png"
	sg.Callback = GenerateEngi
end

hook.Add( "Game.LoadLibrary", LoadLibrary )