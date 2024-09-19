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
import {useToast} from "@/hooks/use-toast.ts";

function UserPage() {
    if(sessionStorage.getItem("userid") != null){
        window.location.href = "/";
    }
    const {toast} = useToast();

    const handleLogin = async () => {
        const username = document.getElementById('username') as HTMLInputElement;
        const password = document.getElementById('password') as HTMLInputElement;

        try {
            const response = await fetch(`${serverLink}/User/${username.value}/${password.value}`, { method: 'GET', mode: 'cors' });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();

            if(data.id === -5){
                return (toast({
                    title: translations.common.error,
                    description: translations.loginmenu.badCredentials,
                    className: "border-2 border-red-400"
                }))
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

        if(password.value !== passwordRepeat.value){
            return (toast({
                title: translations.common.error,
                description: translations.loginmenu.passwordsDontMatch,
                className: "border-2 border-red-400"
            }))
        }

        try {
            const response = await fetch(`${serverLink}/User/${username.value}/${password.value}`, { method: 'GET', mode: 'cors' });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();

            if (data.id !== -5) {
                return (toast({
                    title: translations.common.error,
                    description: translations.loginmenu.userAlreadyExists,
                    className: "border-2 border-red-400"
                }))
            }

            const postData = await fetch(`${serverLink}/User/${username.value}/${password.value}`, {method: 'POST', mode: 'cors'});
            if(postData.ok){
                return (toast({
                    title: translations.common.success,
                    description: translations.loginmenu.successfullyRegistered
                }))
            }
            else{
                return (toast({
                    title: translations.common.error,
                    description: translations.loginmenu.unknownError,
                    className: "border-2 border-red-400"
                }))
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
                                <Input id="password" defaultValue="" type="password" />
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
                                {translations.loginmenu.registerDescription}
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
