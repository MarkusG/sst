import AccountGroup from "./AccountGroup";
import SyncButton from "./SyncButton";

function SidebarAccounts() {
    return (
        <div className="truncate text-sm flex flex-col gap-4">
            <SyncButton text="Sync All" textSize="lg" iconSize="md"/>
            <AccountGroup/>
            <AccountGroup/>
            <AccountGroup/>
        </div>
    );
}

export default SidebarAccounts;
