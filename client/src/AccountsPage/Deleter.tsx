import {useState} from "react";
import useDeleteAccount from "./DeleteAccountCommand.ts";

export interface DeleterProps {
    accountId: string,
    accountName: string
}

export default function Deleter({accountId, accountName}: DeleterProps) {
    const [clicked, setClicked] = useState(false);
    const [input, setInput] = useState('');
    const [submitting, setSubmitting] = useState(false);

    function reset() {
        setClicked(false);
        setInput('');
    }

    const {mutateAsync} = useDeleteAccount(accountId);

    async function submit() {
        setSubmitting(true);
        await mutateAsync();
    }

    return (
        <div className="flex gap-2">
            {clicked &&
                <div className="flex flex-col">
                    <span className="text-sm">Are you sure? Type the account name.</span>
                    <input value={input} autoFocus onChange={e => setInput(e.target.value)} className="border border-gray-300 rounded px-2 mb-2"/>
                    {input === accountName &&
                        <button disabled={submitting} className="px-2 bg-red-500 disabled:bg-red-300 text-red-100 rounded" onClick={submit}>
                            Delete
                        </button>
                    }
                    {input !== accountName &&
                        <button className="px-2 bg-gray-100 text-gray-700 rounded" onClick={reset}>
                            Cancel
                        </button>
                    }
                </div>
            }
            {!clicked &&
                <button className="text-red-500 mr-2" title="Delete" onClick={() => setClicked(true)}>
                    <i className="fa fa-trash"></i>
                </button>
            }
        </div>
    );
}