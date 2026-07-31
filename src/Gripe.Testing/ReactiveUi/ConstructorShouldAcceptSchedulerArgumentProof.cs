// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.ReactiveUi
{
    public sealed class ConstructorShouldAcceptSchedulerArgumentProof : ReactiveUI.ReactiveObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorShouldAcceptSchedulerArgumentProof"/> class.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ConstructorShouldAcceptSchedulerArgumentProof();
        /// </code>
        /// </example>
        public ConstructorShouldAcceptSchedulerArgumentProof()
        {
            SomeMethod();
        }

        private static void SomeMethod()
        {
        }
    }

    public sealed class ConstructorShouldAcceptSchedulerArgumentNoWarningProof : ReactiveUI.ReactiveObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorShouldAcceptSchedulerArgumentNoWarningProof"/> class.
        /// </summary>
        /// <param name="scheduler">Scheduler argument.</param>
        /// <example>
        /// <code>
        /// System.Reactive.Concurrency.IScheduler scheduler = null!;
        /// var proof = new ConstructorShouldAcceptSchedulerArgumentNoWarningProof(scheduler);
        /// </code>
        /// </example>
        public ConstructorShouldAcceptSchedulerArgumentNoWarningProof(System.Reactive.Concurrency.IScheduler scheduler)
        {
        }
    }
}
