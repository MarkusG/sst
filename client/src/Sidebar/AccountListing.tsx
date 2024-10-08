import { AccountResponse } from "../Contracts/Responses";
import { currencyFormatter } from "../Utils";

export interface AccountListingProps {
    account: AccountResponse
}

export default function AccountListing({ account }: AccountListingProps) {
    return (
        <li className="mb-2">
            <p className="truncate text-ellipsis" title={account.name}>{account.name}</p>
            {account.currentBalance && <p className="text-xs">{currencyFormatter.format(account.currentBalance)}</p>}
        </li>
    );
}
