﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
using k8s;
using k8s.Models;
using k8s.Autorest;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Kubernetes.Client;

public interface IAnyResourceKind
{
    Task<HttpOperationResponse<KubernetesList<TResource>>> ListClusterAnyResourceKindWithHttpMessagesAsync<TResource>(
        string group, string version, string plural, string continueParameter = null, string fieldSelector = null,
        string labelSelector = null, int? limit = null, string resourceVersion = null, int? timeoutSeconds = null,
        bool? watch = null, string pretty = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>> customHeaders = null,
        CancellationToken cancellationToken = default) where TResource : k8s.IKubernetesObject;

    public Task<HttpOperationResponse<object>> CreateAnyResourceKindWithHttpMessagesAsync<TResource>(TResource body,
        string group, string version, string namespaceParameter, string plural, string dryRun = default(string),
        string fieldManager = default(string), string pretty = default(string),
        IReadOnlyDictionary<string, IReadOnlyList<string>> customHeaders = null,
        CancellationToken cancellationToken = default(CancellationToken)) where TResource : IKubernetesObject;


    public Task<HttpOperationResponse<object>> PatchAnyResourceKindWithHttpMessagesAsync(V1Patch body, string group,
        string version, string namespaceParameter, string plural, string name, string dryRun = default(string),
        string fieldManager = default(string), bool? force = default(bool?),
        IReadOnlyDictionary<string, IReadOnlyList<string>> customHeaders = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public Task<HttpOperationResponse<object>> DeleteAnyResourceKindWithHttpMessagesAsync(string group, string version,
        string namespaceParameter, string plural, string name, V1DeleteOptions body = default(V1DeleteOptions),
        int? gracePeriodSeconds = default(int?), bool? orphanDependents = default(bool?),
        string propagationPolicy = default(string), string dryRun = default(string),
        IReadOnlyDictionary<string, IReadOnlyList<string>> customHeaders = null,
        CancellationToken cancellationToken = default(CancellationToken));
}
