import React from "react";
import Cookies from 'js-cookie';

class MainMenu extends React.Component
{
    LoggedIn;
    Ready = false;
    UserGames;
    
    componentDidMount()
    {
        fetch('http://' + window.location.host + '/api/user/', {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            this.Ready = true;
            this.setState(data);
            this.LoggedIn = data.Status;

            if(this.LoggedIn)
            {
                fetch('http://' + window.location.host + '/api/games/' + Cookies.get("Username"), {method: 'GET'})
                .then(response => response.json()).then(data =>
                {
                    this.UserGames=data.map((info)=>
                    {
                        var PlayerTurn = false;
                        if(info.Turn && Cookies.get("Username").toLowerCase() === info.PlayerA.toLowerCase()) { PlayerTurn = true; }
                        if(!info.Turn && Cookies.get("Username").toLowerCase() === info.PlayerB.toLowerCase()) { PlayerTurn = true; }

                        return(
                            <tr key={info.GameId} style={{border: "2px solid black"}}>
                                <td> <a href={'http://' + window.location.host + '/game?id=' + info.GameId}> {info.GameId} </a> </td>
                                <td> <a href={'http://' + window.location.host + '/game?id=' + info.GameId}> {info.PlayerA} </a> </td>
                                <td> <a href={'http://' + window.location.host + '/game?id=' + info.GameId}> {info.PlayerB} </a> </td>
                                <td> <a href={'http://' + window.location.host + '/game?id=' + info.GameId}> {PlayerTurn ? "Your's\xa0turn!" : "Opponent's\xa0turn!"} </a> </td>
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
            
            <p> &nbsp; &nbsp; Current Games: </p>
            <table className="games">
                <thead>
                    <tr>
                        <th>Game</th>
                        <th>Player&nbsp;A</th>
                        <th>Player&nbsp;B</th>
                        <th>Turn</th>
                    </tr>
                </thead>
                <tbody>
                    {this.UserGames}
                </tbody>
            </table>
            
            <div style={{clear: "both"}}/><br/>

            <p> &nbsp; &nbsp; New Game: </p>
            <form onSubmit={this.game}>
                <div className="gamebutton">
                    <label style={{float: "left"}}> &nbsp; Opponent's nickname: </label><br/><br/>
                    <input style={{float: "left"}} type="text" name="nickname" required />
                    <input style={{float: "left", marginLeft: "10px"}} type="submit" value="Start Game!"/>
                </div>
            </form>

            <br/> <br/>
            
            <p> &nbsp; &nbsp; You are logged as: </p>
            <form onSubmit={this.logout}>
                <div className="logoutbutton">
                    <input style={{float: "left"}} value={Cookies.get("Username")} disabled/>
                    <input style={{float: "left", marginLeft: "10px"}} type="submit" value="Log Out!"/>
                </div>
            </form>
            
        </div>
    }
    
    forms()
    {
        return <div>

            <form onSubmit={this.register} style={{float: "left"}}>
                <p>Register:</p>
                <div className="registerbutton">
                    <label> Username: </label> <br/>
                    <input type="text" name="username" required /> <br/>
                    <label> Password: </label> <br/>
                    <input type="password" name="password" required /> <br/>
                    <br/> <input type="submit" value="Register!"/>
                </div>
            </form>
            
            <div style={{minWidth: "150px", float: "left"}}>&nbsp;</div>
            
            <form onSubmit={this.login} style={{float: "left"}}>
                <p>Login:</p>
                <div className="loginbutton">
                    <label> Username: </label> <br/>
                    <input type="text" name="username" required /> <br/>
                    <label> Password: </label> <br/>
                    <input type="password" name="password" required /> <br/>
                    <br/> <input type="submit" value="Log In!"/>
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
        
        fetch('http://' + window.location.host + '/api/user/login', { method: 'POST', body: data })
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

        fetch('http://' + window.location.host + '/api/user/register', { method: 'POST', body: data })
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

        fetch('http://' + window.location.host + '/api/user/logout', {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            window.open('http://' + window.location.host, "_self");
        });
    }

    game = (event) =>
    {
        event.preventDefault();

        fetch('http://' + window.location.host + '/api/games/' + Cookies.get("Username") + '/' + event.target[0].value, {method: 'GET'})
        .then(response => response.json()).then(data =>
        {
            window.open('http://' + window.location.host + '/game?id=' + data.GameId, "_self");
        });
    }
}

export default MainMenu;