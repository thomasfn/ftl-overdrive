using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using LuaInterface;

namespace FTLOverdrive.Client.Gamestate
{
    public class ModdingAPI : IState
    {
        private Lua luastate;

        private Dictionary<string, List<LuaFunction>> dctHooks;

        public void OnActivate()
        {
            // Create the lua state
            luastate = new Lua();

            // Setup hooks
            dctHooks = new Dictionary<string, List<LuaFunction>>();

            // Bind functions
            BindFunction("print", "print");

            luastate.NewTable("hook");
            BindFunction("hook.Add", "hook_Add");

            luastate.NewTable("library");
            BindFunction("library.AddWeapon", "library_AddWeapon");
            BindFunction("library.AddSystem", "library_AddSystem");
            BindFunction("library.AddRace", "library_AddRace");
            BindFunction("library.AddShip", "library_AddShip");
            BindFunction("library.CreateAnimation", "library_CreateAnimation");
            BindFunction("library.CreateRoom", "library_CreateRoom");
            BindFunction("library.CreateDoor", "library_CreateDoor");

            // Load lua files
            if (!Directory.Exists("lua")) Directory.CreateDirectory("lua");
            foreach (string name in Directory.GetFiles("lua"))
                luastate.DoFile("lua/" + name);
        }

        public void LoadMods()
        {
            // Validate the directory
            if (!Directory.Exists("mods")) Directory.CreateDirectory("mods");

            // Search for folders
            foreach (var folder in Directory.GetDirectories("mods"))
            {
                string initfile = folder + "/init.lua";
                string modname = Path.GetFileName(folder);
                if (!File.Exists(initfile))
                    Root.Singleton.Log("[" + modname + "] Missing init.lua.");
                else
                    try
                    {
                        luastate.DoFile(initfile);
                    }
                    catch (LuaException ex)
                    {
                        Root.Singleton.Log("[" + modname + "] " + ex.Message);
                    }
            }
        }

        private void BindFunction(string path, string funcname)
        {
            var method = GetType().GetMethod(funcname, BindingFlags.Instance | BindingFlags.NonPublic);
            
            luastate.RegisterFunction(path, this, method);
        }

        public void CallHook(string hookname, params object[] args)
        {
            // Get the hook list
            if (!dctHooks.ContainsKey(hookname)) return;
            var hooks = dctHooks[hookname];

            // Call each hook
            foreach (var func in hooks)
            {
                var results = func.Call(args);
                if ((results != null) && (results.Length > 0)) return;
            }
        }

        #region Lua Binds

        private void print(object obj)
        {
            Root.Singleton.Log("[Lua] " + obj.ToString());
        }

        private void hook_Add(string name, LuaFunction function)
        {
            // Register in the hook
            if (!dctHooks.ContainsKey(name)) dctHooks.Add(name, new List<LuaFunction>());
            dctHooks[name].Add(function);
        }

        private Library.Weapon library_AddWeapon(string name, string type)
        {
            // Check what type of weapon it is
            if (type == "projectile")
            {
                // Create weapon and return it
                var wep = new Library.ProjectileWeapon();
                Root.Singleton.mgrState.Get<Library>().AddWeapon(name, wep);
                return wep;
            }
            return null;
        }

        private Library.System library_AddSystem(string name)
        {
            // Create system and return it
            var sys = new Library.System();
            Root.Singleton.mgrState.Get<Library>().AddSystem(name, sys);
            return sys;
        }

        private Library.Race library_AddRace(string name)
        {
            // Create race and return it
            var race = new Library.Race();
            Root.Singleton.mgrState.Get<Library>().AddRace(name, race);
            return race;
        }

        private Library.Ship library_AddShip(string name)
        {
            // Create ship and return it
            var ship = new Library.Ship();
            Root.Singleton.mgrState.Get<Library>().AddShip(name, ship);
            return ship;
        }

        private Library.Animation library_CreateAnimation(int tilestart, int tileend, int speed)
        {
            return new Library.Animation() { TileStart = tilestart, TileEnd = tileend, Speed = speed };
        }

        private Library.Room library_CreateRoom(int minX, int minY, int maxX, int maxY)
        {
            return new Library.Room() { MinX = minX, MinY = minY, MaxX = maxX, MaxY = maxY };
        }

        private Library.Door library_CreateDoor(float x, float y)
        {
            return new Library.Door() { X = x, Y = y };
        }

        #endregion

        public void OnDeactivate()
        {
            
        }

        public void Think(float delta)
        {
            
        }
    }
}
