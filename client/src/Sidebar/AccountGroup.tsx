import AccountListing from "./AccountListing";
import SyncButton from "./SyncButton";

function AccountGroup() {

    return (
        <div>
            <SyncButton text="Sync" textSize="sm" iconSize="sm"/>
            <ul className="flex flex-col gap-1">
                <AccountListing/>
                <AccountListing/>
            </ul>
        </div>
    );
}

export default AccountGroup;
