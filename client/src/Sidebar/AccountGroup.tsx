import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AccountGroupResonse } from "../Contracts/Responses";
import AccountListing from "./AccountListing";
import SyncButton from "./SyncButton";

export interface AccountGroupProps {
    group: AccountGroupResonse
}

export default function AccountGroup({ group }: AccountGroupProps) {
    const queryClient = useQueryClient();
    const syncMutation = useMutation({
        mutationFn: async () => await fetch(`https://localhost:5001/items/${group.itemId}/sync`, {
            method: "POST"
        }),
        onSuccess: () => {
            queryClient.invalidateQueries(['accounts']);
            queryClient.invalidateQueries(['transactions']);
        }
    });

    return (
        <div>
            <SyncButton text="Sync" textSize="sm" iconSize="sm" onClick={async () => { await syncMutation.mutateAsync() }}/>
            <ul className="flex flex-col gap-1">
                {group.accounts.map(a =>
                    <AccountListing key={a.id} account={a}/>)
                }
            </ul>
        </div>
    );
}
