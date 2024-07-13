//thats a modal, whenever user clicks out of the area we close it
//or when user clicks X button, then we also close it
//if we align items to left is side is 0, else we align to right

interface SidebarProps {
    side: boolean; //0 - left 1- right
    width: number;
    close: () => void;
}

function Sidebar(props: SidebarProps) {
    const { side, width, close } = props;
    let alignment = side == false ? "float-left" : "float-right";

    return (
        <div className={"bg-gray-400 h-full " + alignment}>
            <p>Test</p>
        </div>
    )
}

export default Sidebar;