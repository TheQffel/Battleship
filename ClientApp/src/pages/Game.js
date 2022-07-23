import React from "react";
import Cookies from "js-cookie";

class Game extends React.Component
{
    GameId;
    PlayerBoard;
    OpponentBoard;
    AlreadyPlaced;
    ShipsPositions;
    CurrentTurn;
    LastTurnState;
    LoopLaunched;

    componentDidMount()
    {
        this.GameId = new URL(window.location).searchParams.get('id');

        fetch('http://' + window.location.host + '/api/game/' + this.GameId, {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            if(data.Turn)
            {
                this.CurrentTurn = data.PlayerA;
            }
            else
            {
                this.CurrentTurn = data.PlayerB;
            }
            
            if(data.Winner != null)
            {
                this.CurrentTurn = data.Winner + " Won!";
            }
            
            this.LastTurnState = data.Turn;

            fetch('http://' + window.location.host + '/api/game/' + this.GameId + '/' + Cookies.get("Username"), {method: 'GET'})
            .then(response => response.json()).then(data =>
            {
                var A = data.Player;
                this.PlayerBoard = <tbody onClick={this.playerboardclicked}>
                <tr><td>{A[0][0]}</td><td>{A[0][1]}</td><td>{A[0][2]}</td><td>{A[0][3]}</td><td>{A[0][4]}</td><td>{A[0][5]}</td><td>{A[0][6]}</td><td>{A[0][7]}</td><td>{A[0][8]}</td><td>{A[0][9]}</td></tr>
                <tr><td>{A[1][0]}</td><td>{A[1][1]}</td><td>{A[1][2]}</td><td>{A[1][3]}</td><td>{A[1][4]}</td><td>{A[1][5]}</td><td>{A[1][6]}</td><td>{A[1][7]}</td><td>{A[1][8]}</td><td>{A[1][9]}</td></tr>
                <tr><td>{A[2][0]}</td><td>{A[2][1]}</td><td>{A[2][2]}</td><td>{A[2][3]}</td><td>{A[2][4]}</td><td>{A[2][5]}</td><td>{A[2][6]}</td><td>{A[2][7]}</td><td>{A[2][8]}</td><td>{A[2][9]}</td></tr>
                <tr><td>{A[3][0]}</td><td>{A[3][1]}</td><td>{A[3][2]}</td><td>{A[3][3]}</td><td>{A[3][4]}</td><td>{A[3][5]}</td><td>{A[3][6]}</td><td>{A[3][7]}</td><td>{A[3][8]}</td><td>{A[3][9]}</td></tr>
                <tr><td>{A[4][0]}</td><td>{A[4][1]}</td><td>{A[4][2]}</td><td>{A[4][3]}</td><td>{A[4][4]}</td><td>{A[4][5]}</td><td>{A[4][6]}</td><td>{A[4][7]}</td><td>{A[4][8]}</td><td>{A[4][9]}</td></tr>
                <tr><td>{A[5][0]}</td><td>{A[5][1]}</td><td>{A[5][2]}</td><td>{A[5][3]}</td><td>{A[5][4]}</td><td>{A[5][5]}</td><td>{A[5][6]}</td><td>{A[5][7]}</td><td>{A[5][8]}</td><td>{A[5][9]}</td></tr>
                <tr><td>{A[6][0]}</td><td>{A[6][1]}</td><td>{A[6][2]}</td><td>{A[6][3]}</td><td>{A[6][4]}</td><td>{A[6][5]}</td><td>{A[6][6]}</td><td>{A[6][7]}</td><td>{A[6][8]}</td><td>{A[6][9]}</td></tr>
                <tr><td>{A[7][0]}</td><td>{A[7][1]}</td><td>{A[7][2]}</td><td>{A[7][3]}</td><td>{A[7][4]}</td><td>{A[7][5]}</td><td>{A[7][6]}</td><td>{A[7][7]}</td><td>{A[7][8]}</td><td>{A[7][9]}</td></tr>
                <tr><td>{A[8][0]}</td><td>{A[8][1]}</td><td>{A[8][2]}</td><td>{A[8][3]}</td><td>{A[8][4]}</td><td>{A[8][5]}</td><td>{A[8][6]}</td><td>{A[8][7]}</td><td>{A[8][8]}</td><td>{A[8][9]}</td></tr>
                <tr><td>{A[9][0]}</td><td>{A[9][1]}</td><td>{A[9][2]}</td><td>{A[9][3]}</td><td>{A[9][4]}</td><td>{A[9][5]}</td><td>{A[9][6]}</td><td>{A[9][7]}</td><td>{A[9][8]}</td><td>{A[9][9]}</td></tr>
                </tbody>

                var B = data.Opponent;
                this.OpponentBoard = <tbody onClick={this.opponentboardclicked}>
                <tr><td>{B[0][0]}</td><td>{B[0][1]}</td><td>{B[0][2]}</td><td>{B[0][3]}</td><td>{B[0][4]}</td><td>{B[0][5]}</td><td>{B[0][6]}</td><td>{B[0][7]}</td><td>{B[0][8]}</td><td>{B[0][9]}</td></tr>
                <tr><td>{B[1][0]}</td><td>{B[1][1]}</td><td>{B[1][2]}</td><td>{B[1][3]}</td><td>{B[1][4]}</td><td>{B[1][5]}</td><td>{B[1][6]}</td><td>{B[1][7]}</td><td>{B[1][8]}</td><td>{B[1][9]}</td></tr>
                <tr><td>{B[2][0]}</td><td>{B[2][1]}</td><td>{B[2][2]}</td><td>{B[2][3]}</td><td>{B[2][4]}</td><td>{B[2][5]}</td><td>{B[2][6]}</td><td>{B[2][7]}</td><td>{B[2][8]}</td><td>{B[2][9]}</td></tr>
                <tr><td>{B[3][0]}</td><td>{B[3][1]}</td><td>{B[3][2]}</td><td>{B[3][3]}</td><td>{B[3][4]}</td><td>{B[3][5]}</td><td>{B[3][6]}</td><td>{B[3][7]}</td><td>{B[3][8]}</td><td>{B[3][9]}</td></tr>
                <tr><td>{B[4][0]}</td><td>{B[4][1]}</td><td>{B[4][2]}</td><td>{B[4][3]}</td><td>{B[4][4]}</td><td>{B[4][5]}</td><td>{B[4][6]}</td><td>{B[4][7]}</td><td>{B[4][8]}</td><td>{B[4][9]}</td></tr>
                <tr><td>{B[5][0]}</td><td>{B[5][1]}</td><td>{B[5][2]}</td><td>{B[5][3]}</td><td>{B[5][4]}</td><td>{B[5][5]}</td><td>{B[5][6]}</td><td>{B[5][7]}</td><td>{B[5][8]}</td><td>{B[5][9]}</td></tr>
                <tr><td>{B[6][0]}</td><td>{B[6][1]}</td><td>{B[6][2]}</td><td>{B[6][3]}</td><td>{B[6][4]}</td><td>{B[6][5]}</td><td>{B[6][6]}</td><td>{B[6][7]}</td><td>{B[6][8]}</td><td>{B[6][9]}</td></tr>
                <tr><td>{B[7][0]}</td><td>{B[7][1]}</td><td>{B[7][2]}</td><td>{B[7][3]}</td><td>{B[7][4]}</td><td>{B[7][5]}</td><td>{B[7][6]}</td><td>{B[7][7]}</td><td>{B[7][8]}</td><td>{B[7][9]}</td></tr>
                <tr><td>{B[8][0]}</td><td>{B[8][1]}</td><td>{B[8][2]}</td><td>{B[8][3]}</td><td>{B[8][4]}</td><td>{B[8][5]}</td><td>{B[8][6]}</td><td>{B[8][7]}</td><td>{B[8][8]}</td><td>{B[8][9]}</td></tr>
                <tr><td>{B[9][0]}</td><td>{B[9][1]}</td><td>{B[9][2]}</td><td>{B[9][3]}</td><td>{B[9][4]}</td><td>{B[9][5]}</td><td>{B[9][6]}</td><td>{B[9][7]}</td><td>{B[9][8]}</td><td>{B[9][9]}</td></tr>
                </tbody>

                this.AlreadyPlaced = false;
                for(var i = 0; i < 10; i++)
                {
                    for(var j = 0; j < 10; j++)
                    {
                        if(A[i][j] > 0) { this.AlreadyPlaced = true; }
                    }
                }

                if(!this.AlreadyPlaced)
                {
                    this.ShipsPositions = "";
                    alert("Place your 6-cell-long ship!");
                    this.CurrentTurn = "Place Ships!"
                }

                setTimeout(() => { this.updatestyles(); }, 100);

                if(!this.LoopLaunched)
                {
                    this.LoopLaunched = true;
                    setInterval(() => { this.updatedata(); }, 1000);
                }

                this.setState(data);
            });
        });
    }
    
    updatedata()
    {
        fetch('http://' + window.location.host + '/api/game/' + this.GameId, {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            if(this.LastTurnState !== data.Turn)
            {
                this.LastTurnState = data.Turn;
                this.componentDidMount();
            }
        });
    }

    render()
    {
        return <div>
            <table className="playerboard">
                <thead>
                <tr>
                    <th colSpan="10"><center>Your's&nbsp;board:</center></th>
                </tr>
                </thead>
                {this.PlayerBoard}
            </table>
            <div style={{minWidth: "1%", float: "left"}}>&nbsp;</div>
            <table className="opponentboard">
                <thead>
                <tr>
                    <th colSpan="10"><center>Opponent's&nbsp;board:</center></th>
                </tr>
                </thead>
                {this.OpponentBoard}
            </table>
            <div style={{clear: "both"}}>&nbsp;</div>
            <p> &nbsp; &nbsp; Current Turn: <i>{this.CurrentTurn}</i> </p>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            <a style={{textDecoration: "underline", fontWeight: "bolder", fontSize: "150%"}} href={"http://" + window.location.host}> Back To Main Menu! </a>
        </div>
    }

    updatestyles()
    {
        var elements = document.getElementsByTagName('td');

        for (var i = 0; i < elements.length; i++)
        {
            if(elements[i].innerHTML === "0") { elements[i].style.backgroundColor = "#1050C0"; elements[i].style.color = "#1050C0";}
            if(elements[i].innerHTML === "1") { elements[i].style.backgroundColor = "#109000"; elements[i].style.color = "#109000"; }
            if(elements[i].innerHTML === "2") { elements[i].style.backgroundColor = "#C0B000"; elements[i].style.color = "#C0B000"; }
            if(elements[i].innerHTML === "3") { elements[i].style.backgroundColor = "#C02000"; elements[i].style.color = "#C02000"; }
        }
    }

    playerboardclicked = (event) =>
    {
        if(this.AlreadyPlaced)
        {
            console.log("Error: You already placed your ships!");
            alert("Error: You already placed your ships!");
        }
        else
        {
            var cell = event.target.closest('td');
            if(cell == null) { return; }
            var row = cell.parentElement;
            
            if(cell.innerHTML.toString() === "0")
            {
                cell.innerHTML = "1";
                console.log(row.rowIndex + '' + cell.cellIndex);
                this.ShipsPositions += row.rowIndex-1 + '' + cell.cellIndex;
                
                if(this.ShipsPositions.length === 12)
                {
                    this.ShipsPositions += '-';
                    alert("Place your 5-cell-long ship!");
                }
                if(this.ShipsPositions.length === 23)
                {
                    this.ShipsPositions += '-';
                    alert("Place your 4-cell-long ship!");
                }
                if(this.ShipsPositions.length === 32)
                {
                    this.ShipsPositions += '-';
                    alert("Place your 3-cell-long ship!");
                }
                if(this.ShipsPositions.length === 39)
                {
                    this.ShipsPositions += '-';
                    alert("Place your 2-cell-long ship!");
                }
                if(this.ShipsPositions.length === 44)
                {
                    console.log("Sending request...");
                    fetch('http://' + window.location.host + '/api/game/' + this.GameId + '/' + Cookies.get("Username") + '/?action=' + this.ShipsPositions, {method: 'GET'})
                    .then(response => response.json()).then(data =>
                    {
                        if(data.Error == null)
                        {
                            console.log("Move: " + data.Status);
                            alert("Ships placed!");
                        }
                        else
                        {
                            console.log("Move: " + data.Error);
                            alert("Error: " + data.Error);
                            window.location.reload();
                        }
                    });
                }
            }
        }
        this.updatestyles();
    }

    opponentboardclicked = (event) =>
    {
        var cell = event.target.closest('td');
        if(cell == null) { return; }
        var row = cell.parentElement;
        var move = row.rowIndex-1 + '' + cell.cellIndex;
        
        fetch('http://' + window.location.host + '/api/game/' + this.GameId + '/' + Cookies.get("Username") + '/?action=' + move, {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            if(data.Error == null)
            {
                this.componentDidMount();
                console.log("Move: " + data.Status);
            }
            else
            {
                console.log("Move: " + data.Error);
                alert("Error: " + data.Error);
            }
        });
        this.updatestyles();
    }
}

export default Game;