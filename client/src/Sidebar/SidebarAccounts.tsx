import AccountGroup from "./AccountGroup";
import SyncButton from "./SyncButton";

function SidebarAccounts() {
    return (
        <div className="truncate text-sm flex flex-col gap-4">
            <SyncButton text="Sync All Accounts" textSize="base" iconSize="sm"/>
            <AccountGroup/>
            <AccountGroup/>
            <AccountGroup/>
        </div>
    );
}

export default SidebarAccounts;
