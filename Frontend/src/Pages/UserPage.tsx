

function UserPage() {

    const handleLogin = () => {
        const username = document.getElementById('username') as HTMLInputElement;
        const password = document.getElementById('password') as HTMLInputElement;

        // send get request to backend link at /user/username/password
        fetch(`/user/${username.value}/${password.value}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data === 'success') {
                // redirect to main page
                window.location.href = '/';
            } else {
                // show error message
                alert('Invalid username or password');
            }
        });
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
            <button className="bg-green-400 flex mt-4 justify-end">Login</button>
        </div>
  
    )
}

export default UserPage;