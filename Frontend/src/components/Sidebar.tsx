//thats a modal, whenever user clicks out of the area we close it
//or when user clicks X button, then we also close it
//if we align items to left is side is 0, else we align to right
import { Link } from "react-router-dom";

interface SidebarProps {
    side: boolean; //0 - left 1- right
    width: number;
    close: () => void;
}

function Sidebar(props: SidebarProps) {
    const { side, width, close } = props;
    let alignment = side == false ? " float-left" : " float-right";

    return (
        <div className={"bg-gray-400 h-full w-2/5" + alignment}>
            <p>Test</p>
            <Link to="/login">Login</Link>
        </div>
    )
}

export default Sidebar;