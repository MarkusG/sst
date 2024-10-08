import { useState } from "react";

interface SyncButtonProps {
    text: String
    textSize: String
    iconSize: String
    onClick: () => Promise<void>
}

export default function SyncButton(props: SyncButtonProps) {
    const [syncing, setSyncing] = useState(false);

    async function sync() {
        setSyncing(true);
        await props.onClick();
        setSyncing(false);
    }

    return (
        <button className={`text-gray-600 text-${props.textSize} w-min`} onClick={sync}>
            <i className={`fa fa-${props.iconSize} fa-sync ${syncing ? "fa-spin" : ""}`}></i> {props.text}
        </button>
    );
}
