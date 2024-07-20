import { useState } from "react";

interface SyncButtonProps {
    text: String
    textSize: String
    iconSize: String
}

function SyncButton(props: SyncButtonProps) {
    const [syncing, setSyncing] = useState(false);

    function sync() {
        setSyncing(true);
        setTimeout(() => setSyncing(false), 1000);
    }

    return (
        <button className={`text-gray-600 text-${props.textSize} w-min`} onClick={sync}>
            <i className={`fa fa-${props.iconSize} fa-sync ${syncing ? "fa-spin" : ""}`}></i> {props.text}
        </button>
    );
}

export default SyncButton;
