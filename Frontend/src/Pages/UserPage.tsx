import axios from "axios";
import {serverLink} from "../settings"

function UserPage() {
    if(sessionStorage.getItem("userid") != null){
        //add alert (left bottom screen popup that vanishes after 2 seconds)
        window.location.href = "/";
    }
    
    const handleLogin = async () => {
        const username = document.getElementById('username') as HTMLInputElement;
        const password = document.getElementById('password') as HTMLInputElement;
        
        try {
            const response = await axios.get(`${serverLink}/User/${username.value}/${password.value}`);
            if(response.status !== 200){
                throw new Error('Network response was not ok');
            }

            const data = await response.data;
            console.log(data);
            console.log("Pomyslnie zalogowano!");
            sessionStorage.setItem('userid', data.id);
            window.location.href = '/';

        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
            alert('Failed to login. Please try again later.');
        }
    }

    return (
        <div>
            <p className="Text">Please enter your login credentials</p>
            <fieldset className="Fieldset">
                <label className="border-solid border-2 border-sky-500" htmlFor="username" >
                    Username
                </label>
                <input className="border-solid border-2 border-sky-500" id="username" defaultValue="Username" onFocus = {(e) => {
                    if(e.target.value === "Username") {
                        e.target.value = '';
                    }}} />
            </fieldset>
            <fieldset className="Fieldset">
                <label className="border-solid border-2 border-sky-500" htmlFor="password">
                    Password
                </label>
                <input className="border-solid border-2 border-sky-500" id="password" type="password"/>
            </fieldset>
            <button className="bg-green-400 flex mt-4 justify-end" onClick={handleLogin}>Login</button>
        </div>
    );
}

export default UserPage;
