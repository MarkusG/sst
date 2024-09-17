import { AccountGroupResonse } from "../Contracts/Responses";
import AccountListing from "./AccountListing";
import SyncButton from "./SyncButton";

export interface AccountGroupProps {
    group: AccountGroupResonse
}

function AccountGroup({ group }: AccountGroupProps) {
    return (
        <div>
            <SyncButton text="Sync" textSize="sm" iconSize="sm"/>
            <ul className="flex flex-col gap-1">
                {group.accounts.map(a =>
                    <AccountListing account={a}/>)
                }
            </ul>
        </div>
    );
}

export default AccountGroup;
