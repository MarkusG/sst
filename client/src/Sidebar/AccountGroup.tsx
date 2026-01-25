import { AccountGroupResponse } from "../Contracts/Responses";
import AccountListing from "./AccountListing";
import SyncButton from "./SyncButton";
import useSync from "./SyncCommand.ts";

export interface AccountGroupProps {
  group: AccountGroupResponse;
}

export default function AccountGroup({ group }: AccountGroupProps) {
  const { mutateAsync } = useSync();

  return (
    <div>
      <SyncButton
        text="Sync"
        textSize="sm"
        iconSize="sm"
        onClick={async () => {
          if (group.itemId) await mutateAsync({ itemId: group.itemId });
        }}
      />
      <ul className="flex flex-col gap-1">
        {group.accounts.map((a) => (
          <AccountListing key={a.id} account={a} />
        ))}
      </ul>
    </div>
  );
}
