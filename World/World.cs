using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D
{
    Vector2 screenSize;
    Room[] rooms;
    Player player;

    public override void _Ready()
    {
        this.screenSize = new Vector2((int)ProjectSettings.GetSetting("display/window/size/width"), (int)ProjectSettings.GetSetting("display/window/size/height"));
        this.rooms = this.CacheRooms();

        this.player = this.GetNode<Player>("Player");
        TransitionCamera camera = this.GetNode<TransitionCamera>("Player/TransitionCamera");
    }

    public void ResetRoom()
    {
        // To optimize
        foreach(Room r in this.rooms ?? Array.Empty<Room>())
        {
            // r.Reset();
        }

        this.rooms?.First().Reset();
    }

    private Room FindCurrentRoom(Player player)
    {
        //TODO: optimize
        var roomPosition = new Vector2(player.GlobalPosition / this.screenSize).Floor() * screenSize;
        return this.rooms.FirstOrDefault(x => x.Position == roomPosition);
    }

    private Room[] CacheRooms()
    {
        List<Room> result = new List<Room>();

        for (int i = 0; i < this.GetChildCount(); i++)
        {
            var child = this.GetChild(i);

            if(child is Room room)
            {
                result.Add(room);
            }
        }

        return result.ToArray();
    }
}
