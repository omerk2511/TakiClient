﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki
{
    class CreateGameJSON
    {
        public string code { get; set; }
        public CreateGameJSONArgs args { get; set; }

        public CreateGameJSON(string lobby_name, string player_name, string password)
        {
            this.code = "create_game";
            this.args = new CreateGameJSONArgs(lobby_name, player_name, password);
        }
    }    
    
    class CreateGameJSONArgs
    {
        public string lobby_name { get; set; }
        public string player_name { get; set; }
        public string password { get; set; }

        public CreateGameJSONArgs(string lobby_name, string player_name, string password)
        {
            this.lobby_name = lobby_name;
            this.player_name = player_name;
            this.password = password;
        }
    }
    
    class JoinGameJSON
    {
        public string code { get; set; }
        public JoinGameJSONArgs args { get; set; }

        public JoinGameJSON(string game_id, string player_name, string password)
        {
            this.code = "join_game";
            this.args = new JoinGameJSONArgs(int.Parse(game_id), player_name, password);
        }
    }    
    
    class JoinGameJSONArgs
    {
        public int game_id { get; set; }
        public string player_name { get; set; }
        public string password { get; set; }

        public JoinGameJSONArgs(int game_id, string player_name, string password)
        {
            this.game_id = game_id;
            this.player_name = player_name;
            this.password = password;
        }
    }
    
    class StartGameJSON
    {
        public string code { get; set; }
        public StartGameJSONArgs args { get; set; }

        public StartGameJSON(string jwt)
        {
            this.code = "start_game";
            this.args = new StartGameJSONArgs(jwt);
        }
    }    
    
    class StartGameJSONArgs
    {
        public string jwt { get; set; }

        public StartGameJSONArgs(string jwt)
        {
            this.jwt = jwt;
        }
    }
}
