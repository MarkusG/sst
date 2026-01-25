import AccountGroup from "./AccountGroup";
import SyncButton from "./SyncButton";
import LoadingIcon from "../LoadingIcon/LoadingIcon";
import useGetGroupedAccounts from "./GetGroupedAccountsQuery.ts";

export default function SidebarAccounts() {
  const { data, error, isLoading } = useGetGroupedAccounts();

  if (isLoading) {
    return (
      <div className="w-min m-auto">
        <LoadingIcon />
      </div>
    );
  }

  if (!!error) {
    return <p className="text-sm">{error.toString()}</p>;
  }

  return (
    <div className="w-52 truncate text-sm flex flex-col gap-4">
      <SyncButton
        text="Sync All Accounts"
        textSize="base"
        iconSize="sm"
        onClick={() => {}}
      />
      {data?.groups.map((g) => (
        <AccountGroup key={g.itemId} group={g} />
      ))}
    </div>
  );
}
