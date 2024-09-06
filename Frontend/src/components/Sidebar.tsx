import { Link } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import translations from "../translations/pl.json";

interface SidebarProps {
    children?: React.ReactNode;
}

function Sidebar(props: SidebarProps) {
    const sidebarRef = useRef<HTMLDivElement>(null);
    const [isOpen, setIsOpen] = useState(false);
    const [width, setWidth] = useState(window.innerWidth);
    const isLoggedIn = sessionStorage.getItem("userid") !== null;
    const visibleForLoggedCSS = (isLogged: boolean) => (isLogged ? '' : 'hidden');

    const toggleIsOpen = () => {
        setIsOpen(!isOpen);
    };

    const handleClickOutside = (event: MouseEvent) => {
        if (sidebarRef.current && !sidebarRef.current.contains(event.target as Node)) {
            setIsOpen(false);
        }
    };

    const handleLogout = () => {
        sessionStorage.removeItem("userid");
        window.location.href = "/login";
    }

    useEffect(() => {
        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    useEffect( () => {
        window.addEventListener('resize', () => {
            setWidth(window.innerWidth);
        });

        return () => {
            window.removeEventListener('resize', () => {
                setWidth(window.innerWidth);
            });
        }
    });


    const screenWidth = width;
    const sideBarWidth = screenWidth * 0.2;
    const mainSizeWidth = screenWidth - sideBarWidth - 20;
    const mainSize = mainSizeWidth.toString() + "px";

    return (
        <>
            {/* Backdrop */}
            {isOpen && (
                <div
                    className="fixed inset-0 bg-black bg-opacity-50"
                    style={{ zIndex: 998 }}
                    onClick={toggleIsOpen}
                />
            )}

            {/* Sidebar Modal */}
            {isLoggedIn && (
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
                        <Link to="/" className={`block mb-4 ${visibleForLoggedCSS(isLoggedIn)}`} onClick={toggleIsOpen}>{translations.sidebar.home}</Link>
                        <Link to="/login" className={`block mb-4 ${visibleForLoggedCSS(!isLoggedIn)}`} onClick={toggleIsOpen}>{translations.sidebar.login}</Link>
                        <Link to="/register" className={`block mb-4 ${visibleForLoggedCSS(!isLoggedIn)}`} onClick={toggleIsOpen}>{translations.sidebar.register}</Link>
                        <Link to="/add" className={`block mb-4 ${visibleForLoggedCSS(isLoggedIn)}`} onClick={toggleIsOpen}>{translations.sidebar.addReceipt}</Link>
                        <Link to="/receipts" className={`block mb-4 ${visibleForLoggedCSS(isLoggedIn)}`} onClick={toggleIsOpen}>{translations.sidebar.myReceipts}</Link>
                        <p onClick={handleLogout} className={`absolute bottom-1.5 ${visibleForLoggedCSS(isLoggedIn)} cursor-pointer`}>{translations.sidebar.logout}</p>
                    </div>
                )}
            </div>)}

            {/* Toggle Button */}
            {!isOpen && isLoggedIn &&(
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
                style={{marginLeft: isLoggedIn ? '20%' : '0%',
                    width: isLoggedIn ? mainSize : window.innerWidth,
                }}>
                {props.children}
            </main>
        </>
    );
}

export default Sidebar;
