import React from "react";
import Cookies from 'js-cookie';

class MainMenu extends React.Component
{
    LoggedIn;
    Ready = false;
    UserGames;
    
    componentDidMount()
    {
        fetch('https://' + window.location.host + '/api/user/', {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            this.Ready = true;
            this.setState(data);
            this.LoggedIn = data.Status;

            if(this.LoggedIn)
            {
                fetch('https://' + window.location.host + '/api/games/' + Cookies.get("Username"), {method: 'GET'})
                .then(response => response.json()).then(data =>
                {
                    this.UserGames=data.map((info)=>
                    {
                        var PlayerTurn = false;
                        if(info.Turn && Cookies.get("Username").toLowerCase() === info.PlayerA.toLowerCase()) { PlayerTurn = true; }
                        if(!info.Turn && Cookies.get("Username").toLowerCase() === info.PlayerB.toLowerCase()) { PlayerTurn = true; }

                        return(
                            <tr key={info.GameId}>
                                <td> <a href={'https://' + window.location.host + '/game?id=' + info.GameId}> {info.GameId} </a> </td>
                                <td> <a href={'https://' + window.location.host + '/game?id=' + info.GameId}> {info.PlayerA} </a> </td>
                                <td> <a href={'https://' + window.location.host + '/game?id=' + info.GameId}> {info.PlayerB} </a> </td>
                                <td> <a href={'https://' + window.location.host + '/game?id=' + info.GameId}> {PlayerTurn ? "Your turn!" : "Opponent's turn!"} </a> </td>
                            </tr>
                        )
                    });

                    this.Ready = true;
                    this.setState(data);
                });
            }
        });
    }

    render()
    {
        if(this.Ready)
        {
            if(this.LoggedIn)
            {
                return this.panel();
            }
            else
            {
                return this.forms();
            }
        }
        return <h1>Loading, please wait...</h1>
    }
    
    panel()
    {
        return <div>
            
            <table className="games">
                <thead>
                    <tr>
                        <th>Game</th>
                        <th>Player 1</th>
                        <th>Player 2</th>
                        <th>Turn</th>
                    </tr>
                </thead>
                <tbody>
                    {this.UserGames}
                </tbody>
            </table>

            <p>Logout:</p>
            <form onSubmit={this.logout}>
                <div className="logoutbutton">
                    <input type="submit" />
                </div>
            </form>

            <p>New Game:</p>
            <form onSubmit={this.game}>
                <div className="forminput">
                    <label>Opponent's nickname: </label>
                    <input type="text" name="nickname" required />
                </div>
                <div className="gamebutton">
                    <input type="submit" />
                </div>
            </form>
            
        </div>
    }
    
    forms()
    {
        return <div>
            
            <p>Login:</p>
            <form onSubmit={this.login}>
                <div className="forminput">
                    <label>Username </label>
                    <input type="text" name="username" required />
                </div>
                <div className="forminput">
                    <label>Password </label>
                    <input type="password" name="password" required />
                </div>
                <div className="loginbutton">
                    <input type="submit" />
                </div>
            </form>
        
            <p>Register:</p>
            <form onSubmit={this.register}>
                <div className="forminput">
                    <label>Username </label>
                    <input type="text" name="username" required />
                </div>
                <div className="forminput">
                    <label>Password </label>
                    <input type="password" name="password" required />
                </div>
                <div className="registerbutton">
                    <input type="submit" />
                </div>
            </form>
            
        </div>
    }
    
    login = (event) =>
    {
        event.preventDefault();

        var data = new FormData();
        data.append('Username', event.target[0].value);
        data.append('Password', event.target[1].value);
        
        fetch('https://' + window.location.host + '/api/user/login', { method: 'POST', body: data })
        .then(response => response.json()).then(data =>
        {
            if(data.Status)
            {
                console.log("Sucessfully logged in!");
                window.location.reload(false);
            }
            else
            {
                console.log("Wrong username or password!");
            }
        });
    }


    register = (event) =>
    {
        event.preventDefault();

        var data = new FormData();
        data.append('Username', event.target[0].value);
        data.append('Password', event.target[1].value);

        fetch('https://' + window.location.host + '/api/user/register', { method: 'POST', body: data })
        .then(response => response.json()).then(data =>
        {
            if(data.Status)
            {
                console.log("Sucessfully registered!");
                window.location.reload(false);
            }
            else
            {
                console.log("User already exists, or nickname / password doesn't meet requirements!");
            }
        });
    }

    logout = (event) =>
    {
        event.preventDefault();

        fetch('https://' + window.location.host + '/api/user/logout', {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            window.open('https://' + window.location.host, "_self");
        });
    }

    game = (event) =>
    {
        event.preventDefault();

        fetch('https://' + window.location.host + '/api/games/' + Cookies.get("Username") + '/' + event.target[0].value, {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            window.open('https://' + window.location.host + '/game?id=' + data.GameId, "_self");
        });
    }
}

export default MainMenu;