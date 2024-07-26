import React, { useState } from "react";
import Sidebar from "./Sidebar";

interface NavbarProps{
    //take multiple children
    children?: React.ReactNode;
}

function Navbar(props: NavbarProps) {
    const { children } = props;

    let [showbar, setShowbar] = useState(false);

    let sideBar = <Sidebar rightSide={false} width={300} close={() => setShowbar(false)} />;

    const handleShowbarOpening = () => {
        setShowbar(true);
    }

    return (
        <>
            <div className="w-screen bg-green-600 border border-black">
                <button className="text-xl text-black float-left" onClick={handleShowbarOpening}>≡</button>
                <span>""</span>
            </div>
            {showbar && sideBar}
            {children}
        </>
    )
}

export default Navbar;