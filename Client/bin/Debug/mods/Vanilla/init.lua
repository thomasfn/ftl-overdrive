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
	
	
end
hook.Add( "Game.LoadLibrary", LoadLibrary )