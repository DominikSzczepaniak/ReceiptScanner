import { Link } from "react-router-dom";
import { useEffect, useRef, useState } from "react";

interface SidebarProps {
    children?: React.ReactNode;
}

function Sidebar(props: SidebarProps) {
    const sidebarRef = useRef<HTMLDivElement>(null);
    const [isOpen, setIsOpen] = useState(false);  // Start with the sidebar closed
    let screenWidth = window.innerWidth;
    let sideBarWidth = screenWidth * 0.2;
    let mainSizeWidth = screenWidth - sideBarWidth - 20;
    let mainSize = mainSizeWidth.toString() + "px";

    const toggleIsOpen = () => {
        setIsOpen(!isOpen);
    };

    const handleClickOutside = (event: MouseEvent) => {
        if (sidebarRef.current && !sidebarRef.current.contains(event.target as Node)) {
            setIsOpen(false);
        }
    };

    useEffect(() => {
        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    return (
        <>
            {/* Backdrop */}
            {isOpen && (
                <div
                    className="fixed inset-0 bg-black bg-opacity-50"
                    style={{ zIndex: 998 }}
                    onClick={toggleIsOpen} // Clicking the backdrop will close the sidebar
                />
            )}

            {/* Sidebar Modal */}
            <div
                ref={sidebarRef}
                className={`fixed top-0 left-0 h-screen bg-gray-400 ${isOpen ? 'w-1/5' : 'w-0'} transition-all duration-300 overflow-hidden`}
                style={{ zIndex: 999 }}
            >
                {isOpen && (
                    <div className="relative h-full p-4">
                        <button
                            className="absolute top-4 right-4 text-xl text-black"
                            onClick={toggleIsOpen}
                        >
                            &times;
                        </button>
                        <Link to="/" className="block mb-4" onClick={toggleIsOpen}>Home</Link>
                        <Link to="/login" className="block mb-4" onClick={toggleIsOpen}>Login</Link>
                    </div>
                )}
            </div>

            {/* Toggle Button */}
            {!isOpen && (
                <button
                    className="text-xl text-black fixed top-4 left-4"
                    style={{ zIndex: 1000 }}
                    onClick={toggleIsOpen}
                >
                    ≡
                </button>
            )}

            <main 
                className={`transition-all duration-300 fixed h-screen`} 
                style={{marginLeft: '20%',
                    width: mainSize,
                }}>
                {props.children}
            </main>
        </>
    );
}

export default Sidebar;
