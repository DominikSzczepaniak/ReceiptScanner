import {serverLink} from "../settings"

function UserPage() {

    const handleLogin = async () => {
        const username = document.getElementById('username') as HTMLInputElement;
        const password = document.getElementById('password') as HTMLInputElement;
        
        try {
            const response = await fetch(`${serverLink}/User/${username.value}/${password.value}`, {
                method: 'GET', 
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();

            if (response.ok) {
                // redirect to main page
                window.location.href = '/';
                sessionStorage.setItem('userid', data.id);
            } else {
                // show error message
                alert('Invalid username or password');
            }
        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
            alert('Failed to login. Please try again later.');
        }
    }

    return (
        <div>
            <p className="Text">Please enter your login credentials</p>
            <fieldset className="Fieldset">
                <label className="border-solid border-2 border-sky-500" htmlFor="username">
                    Username
                </label>
                <input className="border-solid border-2 border-sky-500" id="username" defaultValue="Username" />
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
