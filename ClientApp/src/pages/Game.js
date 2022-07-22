import React from "react";

class Game extends React.Component
{
    GameId;
    
    render()
    {
        this.GameId = new URL(window.location).searchParams.get('id')
        
        return <h1>Game: {this.GameId}</h1>
    }
}

export default Game;