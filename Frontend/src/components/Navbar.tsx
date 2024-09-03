import React, { useState } from "react";
import Sidebar from "./Sidebar";

interface NavbarProps{
    children?: React.ReactNode;
}

function Navbar(props: NavbarProps) {
    const { children } = props;
    const [showbar, setShowbar] = useState(false);

    const handleShowbarOpening = () => {
        setShowbar(true);
    }

    return (
        <>
            <div className="w-screen bg-green-600 border border-black">
                <button className="text-xl text-black float-left" onClick={handleShowbarOpening}>≡</button>
                <span>""</span>
            </div>
            {showbar && <Sidebar />}
            {children}
        </>
    )
}

export default Navbar;