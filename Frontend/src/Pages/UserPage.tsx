import axios from "axios";
import {serverLink} from "../settings"
import translations from "../translations/pl.json";
import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
    Tabs,
    TabsContent,
    TabsList,
    TabsTrigger,
} from "@/components/ui/tabs"

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
            if(data.id === -5){
                //show toast incorrect credentials
                return;
            }
            sessionStorage.setItem('userid', data.id);
            window.location.href = '/';

        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
            alert('Failed to login. Please try again later.');
        }
    }

    const handleRegister = async () => {
        const username = document.getElementById('username') as HTMLInputElement;
        // const email = document.getElementById('email') as HTMLInputElement;
        const password = document.getElementById('password') as HTMLInputElement;
        const passwordRepeat = document.getElementById('passwordRepeat') as HTMLInputElement;

        if(password !== passwordRepeat){
            //toast error - wrong passwords
        }

        try {
            const response = await axios.get(`${serverLink}/User/${username.value}/${password.value}`);
            if (response.status !== 200) {
                throw new Error('Network response was not ok');
            }

            const data = await response.data;
            if (data.id !== -5) {
                //show toast users exists
                return;
            }

            const postData = await fetch(`${serverLink}/User/${username.value}/${password.value}`, {method: 'POST', mode: 'cors'});
            if(postData.ok){
                //toast correct
                return;
            }
            else{
                //toast error
            }
        } catch(error) {
            console.error('There was a problem with the fetch operation:', error);
        }


    }

    return (
        <div className="w-full h-screen flex justify-center content-center items-center">
            <Tabs defaultValue="LoginMenu" className="w-[400px]">
                <TabsList className="grid w-full grid-cols-2">
                    <TabsTrigger value="login">{translations.loginmenu.login}</TabsTrigger>
                    <TabsTrigger value="register">{translations.loginmenu.register}</TabsTrigger>
                </TabsList>
                <TabsContent value="login">
                    <Card>
                        <CardHeader>
                            <CardTitle>{translations.loginmenu.login}</CardTitle>
                            <CardDescription>
                                {translations.loginmenu.loginDescription}
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-2">
                            <div className="space-y-1">
                                <Label htmlFor="username">{translations.loginmenu.username}</Label>
                                <Input id="username" defaultValue="" />
                            </div>
                            <div className="space-y-1">
                                <Label htmlFor="password">{translations.loginmenu.password}</Label>
                                <Input id="password" defaultValue="" />
                            </div>
                        </CardContent>
                        <CardFooter>
                            <Button onClick={handleLogin}>{translations.loginmenu.login}</Button>
                        </CardFooter>
                    </Card>
                </TabsContent>
                <TabsContent value="register">
                    <Card>
                        <CardHeader>
                            <CardTitle>{translations.loginmenu.register}</CardTitle>
                            <CardDescription>
                                {translations.loginmenu.registeDescription}
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-2">
                            <div className="space-y-1">
                                <Label htmlFor="username">{translations.loginmenu.username}</Label>
                                <Input id="username"/>
                            </div>
                            {/*<div className="space-y-1">*/}
                            {/*    <Label htmlFor="email">{translations.loginmenu.email}</Label>*/}
                            {/*    <Input id="email"/>*/}
                            {/*</div>*/}
                            <div className="space-y-1">
                                <Label htmlFor="password">{translations.loginmenu.password}</Label>
                                <Input id="password" type="password"/>
                            </div>
                            <div className="space-y-1">
                                <Label htmlFor="passwordRepeat">{translations.loginmenu.repeatPassword}</Label>
                                <Input id="passwordRepeat" type="password"/>
                            </div>
                        </CardContent>
                        <CardFooter>
                            <Button onClick={handleRegister}>{translations.loginmenu.register}</Button>
                        </CardFooter>
                    </Card>
                </TabsContent>
            </Tabs>
        </div>
    );
}

export default UserPage;
